using UnityEngine;
using System.ComponentModel;
using System.Runtime.ExceptionServices;
using System;

namespace WormGame.Core.Extensions
{
    public static class Vector3Extensions
    {
        // This extension method tries to parse a string as a Vector3 and returns true if successful, false otherwise
        public static bool TryParse(this string s, out Vector3 result)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Vector3));

            // Try to convert the string to a Vector3
            try
            {
                result = (Vector3)converter.ConvertFromString(s);
                return true;
            }
            catch (Exception ex)
            {
                // Capture the exception and its stack trace
                ExceptionDispatchInfo edi = ExceptionDispatchInfo.Capture(ex);

                result = default(Vector3);

                edi.Throw();

                // This line will never be reached, but is required to satisfy the compiler
                return false;
            }
        }
    }
}