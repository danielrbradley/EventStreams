﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18010
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EventStreams.Persistence.Riak.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExceptionStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EventStreams.Persistence.Riak.Resources.ExceptionStrings", typeof(ExceptionStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to commit {0:N0} object(s). Review the inner exception(s) for further information..
        /// </summary>
        internal static string Commit_objects_unexpectantly_failed {
            get {
                return ResourceManager.GetString("Commit_objects_unexpectantly_failed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pointer object ({0}) exists but it does not contain the expected link ({1}). The bucket is either invalid, corrupted or has been tampered with by a foreign system..
        /// </summary>
        internal static string Corrupt_pointer {
            get {
                return ResourceManager.GetString("Corrupt_pointer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Head pointer could not be updated..
        /// </summary>
        internal static string Head_pointer_update_failed {
            get {
                return ResourceManager.GetString("Head_pointer_update_failed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to store {0:N0} object(s) (of {1:N0}) as a self-linked but dereferenced unit..
        /// </summary>
        internal static string Object_creation_pre_commitment_failed {
            get {
                return ResourceManager.GetString("Object_creation_pre_commitment_failed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to wire-up the links of {0:N0} object(s); they are self-linked but will remain dereferenced..
        /// </summary>
        internal static string Object_wireup_failed {
            get {
                return ResourceManager.GetString("Object_wireup_failed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tail pointer could not be updated..
        /// </summary>
        internal static string Tail_pointer_update_failed {
            get {
                return ResourceManager.GetString("Tail_pointer_update_failed", resourceCulture);
            }
        }
    }
}
