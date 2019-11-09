using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace EmptyService.CommonEntities
{
    public struct Ip : IComparable,
                       IComparable<Ip>,
                       IEquatable<Ip>
    {
        public Ip(IPAddress ipAddress)
            : this(ipAddress.GetAddressBytes()) { }

        public Ip(byte[] bytes)
            : this(bytes[0], bytes[1], bytes[2], bytes[3]) { }

        public Ip(string address)
        {
            var bytes = address.Split('.').Select(byte.Parse).ToArray();

            if (bytes.Length != 4)
            {
                throw new ArgumentException($"Expected ipv4 format, but {address} was provided");
            }

            a = bytes[0];
            b = bytes[1];
            c = bytes[2];
            d = bytes[3];
        }

        public Ip(int a, int b, int c, int d)
        {
            var data = new[]
            {
                a, b, c, d,
            };

            if (!data.All(x => x.IsInCloseRange(byte.MinValue, byte.MaxValue)))
            {
                throw new
                    ArgumentException($"Ip could contains only bytes (range {byte.MinValue}-{byte.MaxValue}), but {a}.{b}.{c}.{d} was provided");
            }

            this.a = (byte)a;
            this.b = (byte)b;
            this.c = (byte)c;
            this.d = (byte)d;
        }

        private readonly byte a;

        private readonly byte b;

        private readonly byte c;

        private readonly byte d;

        public Uri ToUri()
        {
            return new Uri(ToString());
        }

        public override string ToString()
        {
            return $"{a}.{b}.{c}.{d}";
        }

#region comparer

        private static readonly IpComparer LocalComparer = new IpComparer();

        public IComparer<Ip> Comparer => LocalComparer;

        public IEqualityComparer<Ip> EqualityComparer => LocalComparer;

        private sealed class IpComparer : IComparer,
                                          IComparer<Ip>,
                                          IEqualityComparer,
                                          IEqualityComparer<Ip>
        {
            public int Compare(Ip lhs, Ip rhs)
            {
                const int equals = 0;
                const int notEquals = 1; // introduce less/greater constants if you need them

                if (lhs == default &&
                    rhs == default)
                {
                    return equals;
                }

                if (lhs == default ||
                    rhs == default)
                {
                    return notEquals;
                }

                var areEquals = lhs.a == rhs.a &&
                                lhs.b == rhs.b &&
                                lhs.c == rhs.c &&
                                lhs.d == rhs.d;

                return areEquals ? equals : notEquals;
            }

            public int Compare(object lhs, object rhs)
            {
                const int equals = 0;
                const int notEquals = 1;

                if (lhs is null &&
                    rhs is null)
                {
                    return equals;
                }

                if (lhs is null ||
                    rhs is null)
                {
                    return notEquals;
                }

                if (ReferenceEquals(lhs, rhs))
                {
                    return equals;
                }

                if (lhs is Ip lhsItem &&
                    rhs is Ip rhsItem)
                {
                    return Compare(lhsItem, rhsItem);
                }

                return notEquals;
            }

            public bool Equals(Ip lhs, Ip rhs)
            {
                return Compare(lhs, rhs) == 0;
            }

            public int GetHashCode(Ip item)
            {
                unchecked
                {
                    unchecked
                    {
                        // Ensure that there are no warnings here
                        // Ensure that there are no mutable fields in hash code calculations
                        return (item.a ^ item.b) & (item.c ^ item.d);
                    }
                }
            }

            public int GetHashCode(object obj)
            {
                if (obj is Ip item)
                {
                    return GetHashCode(item);
                }

                return obj.GetHashCode();
            }

            bool IEqualityComparer.Equals(object lhs, object rhs)
            {
                return Compare(lhs, rhs) == 0;
            }
        }

        public int CompareTo(object obj)
        {
            return LocalComparer.Compare(this, obj);
        }

        public int CompareTo(Ip other)
        {
            return LocalComparer.Compare(this, other);
        }

        public override bool Equals(object obj)
        {
            return LocalComparer.Compare(this, obj) == 0;
        }

        public bool Equals(Ip other)
        {
            return LocalComparer.Compare(this, other) == 0;
        }

        public override int GetHashCode()
        {
            return EqualityComparer.GetHashCode(this);
        }

        public static bool operator ==(Ip lhs, Ip rhs)
        {
            return LocalComparer.Compare(lhs, rhs) == 0;
        }

        public static bool operator !=(Ip lhs, Ip rhs)
        {
            return LocalComparer.Compare(lhs, rhs) != 0;
        }

#endregion
    }
}