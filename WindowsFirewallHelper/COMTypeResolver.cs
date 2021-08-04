using System;
using System.Runtime.InteropServices;
using WindowsFirewallHelper.InternalHelpers;

namespace WindowsFirewallHelper
{
    /// <summary>
    ///     Contains methods facilitating access to local and remove COM+ types
    /// </summary>
    public class COMTypeResolver
    {
        /// <summary>
        ///     Gets the machine name assigned to this instance or <see langword="null"/>
        /// </summary>
        public string MachineName { get; }

        /// <summary>
        ///     Creates a new instance of <see cref="COMTypeResolver"/> allowing COM+ connection to remote machines
        /// </summary>
        /// <param name="machineName">The remote machine name or IP address</param>
        public COMTypeResolver(string machineName)
        {
            MachineName = machineName;
        }

        /// <summary>
        ///     Creates a new instance of <see cref="COMTypeResolver"/> providing COM+ connection to local machine
        /// </summary>
        public COMTypeResolver() : this(null)
        {
            
        }

        internal T CreateInstance<T>()
        {
            if (!typeof(T).IsInterface)
            {
                throw new ArgumentException("Invalid generic type passed.", nameof(T));
            }

            try
            {
                var progId = ComClassProgIdAttribute.GetClassProgId<T>();

                if (!string.IsNullOrWhiteSpace(progId))
                {
                    var typeByProgId = Type.GetTypeFromProgID(progId, MachineName, false);

                    if (typeByProgId != null)
                    {
                        try
                        {
                            return (T)Activator.CreateInstance(typeByProgId);
                        }
                        catch (COMException)
                        {
                            if (MachineName == null)
                            {
                                throw;
                            }
                        }
                    }
                }

                var typeByClassId = Type.GetTypeFromCLSID(typeof(T).GUID, MachineName, false);

                if (typeByClassId != null)
                {
                    try
                    {
                        return (T)Activator.CreateInstance(typeByClassId);
                    }
                    catch (COMException)
                    {
                        if (MachineName == null)
                        {
                            throw;
                        }
                    }
                }
            }
            catch (COMException e)
            {
                throw new NotSupportedException("Can not create a new instance of this interface in current environment.", e);
            }

            throw new NotSupportedException("Can not create a new instance of this interface in current environment.");
        }

        internal bool IsSupported<T>()
        {
            if (!typeof(T).IsInterface)
            {
                throw new ArgumentException("Invalid generic type passed.", nameof(T));
            }

            try
            {
                return CreateInstance<T>() != null;
            }
            catch
            {
                return false;
            }
        }
    }
}