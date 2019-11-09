using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Xml;
using EmptyService.CommonEntities.Exceptions;
using EmptyService.CommonEntities.Pathes;

namespace EmptyService.CommonEntities
{
    // ReSharper disable once AllowPublicClass
    public static class Extensions
    {
        public static bool ContainsOneOf(this string item, params string[] strings)
        {
            return strings.Where(item.Contains).Any();
        }

        public static bool ContainsOneOf<T>(this IEnumerable<T> collection, params T[] items)
        {
            return collection.Any(items.Contains);
        }

        public static bool EndsWithOneOf(this string item, string[] suffixes)
        {
            return suffixes.Where(item.EndsWith).Any();
        }

        public static string GetXPath(this XmlNode root)
        {
            void Impl(StringBuilder path, XmlNode current)
            {
                path.Insert(0, $"/{current.Name}");

                if (!(current.ParentNode is XmlElement parentElement))
                {
                    return;
                }

                XmlNodeList siblings = null;

                if (current.NodeType != XmlNodeType.Text)
                {
                    siblings = parentElement.SelectNodes(current.Name);
                }

                // There's more than 1 element with the same name
                if (siblings != null &&
                    siblings.Count > 1)
                {
                    var position = 1;

                    foreach (XmlElement sibling in siblings)
                    {
                        if (sibling == current)
                        {
                            break;
                        }

                        position++;
                    }

                    path.Append($"[{position}]");
                }

                Impl(path, parentElement);
            }

            var builder = new StringBuilder();
            Impl(builder, root);

            return builder.ToString();
        }

        public static bool IsInCloseRange<T>(this T value, T min, T max)
            where T : IComparable
        {
            return value.CompareTo(min) >= 0 &&
                   value.CompareTo(max) <= 0;
        }

        public static bool IsInOpenRange<T>(this T value, T min, T max)
            where T : IComparable
        {
            return value.CompareTo(min) > 0 &&
                   value.CompareTo(max) < 0;
        }

        public static AbsolutePath ToAbsolutePath(this string path)
        {
            if (AbsolutePath.IsExistingFile(path))
            {
                return path.ToFilePath();
            }

            if (AbsolutePath.IsExistingDirectory(path))
            {
                return path.ToDirectoryPath();
            }

            throw new
                ImpossibleSituationException($"Entity '{path}' is a file or a directory, but a third option was hit");
        }

        public static DirectoryPath ToDirectoryPath(this DirectoryInfo info)
        {
            return new DirectoryPath(info.FullName);
        }

        public static DirectoryPath ToDirectoryPath(this string path)
        {
            return new DirectoryPath(path);
        }

        public static FilePath ToFilePath(this FileInfo info)
        {
            return new FilePath(info.FullName);
        }

        public static FilePath ToFilePath(this string path)
        {
            return new FilePath(path);
        }

        public static Uri ToUri(this string address)
        {
            return new Uri(address);
        }

        public static Ip ToIp(this string address)
        {
            return new Ip(address);
        }

        public static SecureString ToSecureString(this string value)
        {
            var secureStr = new SecureString();

            if (value.Length <= 0)
            {
                return secureStr;
            }

            foreach (var c in value.ToCharArray())
            {
                secureStr.AppendChar(c);
            }

            return secureStr;
        }

        public static long ToTimeStamp(this DateTime dateTime)
        {
            return ((DateTimeOffset)dateTime).ToUnixTimeMilliseconds();
        }

        public static string ToUnsecureString(this SecureString value)
        {
            var unmanagedString = IntPtr.Zero;

            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(value);

                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static void Trim(this StringBuilder sb, bool saveFirstSpace, bool saveLastSpace)
        {
            for (var i = 0; i < sb.Length - 1; i++)
            {
                if (saveFirstSpace &&
                    char.IsWhiteSpace(sb[i]) &&
                    !char.IsWhiteSpace(sb[i + 1]))
                {
                    break;
                }

                if (char.IsWhiteSpace(sb[i]))
                {
                    sb.Remove(i, 1);
                }
            }

            for (var i = sb.Length - 1; i > 1; i--)
            {
                if (saveLastSpace &&
                    char.IsWhiteSpace(sb[i]) &&
                    !char.IsWhiteSpace(sb[i - 1]))
                {
                    break;
                }

                if (char.IsWhiteSpace(sb[i]))
                {
                    sb.Remove(i, 1);
                }
            }
        }

        public static bool IsEquals<T>(this IEnumerable<T> origin, IEnumerable<T> other)
        {
            return origin.OrderBy(_ => _).SequenceEqual(other.OrderBy(_ => _));
        }

        public static object CreateInstance(this Type type)
        {
            var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);

            if (!constructors.Any())
            {
                throw new Exception();
            }

            var constructor = constructors.First();
            var parameters = constructor.GetParameters();

            return constructor.Invoke(parameters.Select(x => GetDefault(x.ParameterType)).ToArray());
        }

        public static T CreateInstance<T>()
        {
            return (T)CreateInstance(typeof(T));
        }

        private static object GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}