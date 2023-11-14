using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Linq.Expressions;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace HotelBooking.Application.Helpers;
public static class UtilitiesExtensions
{

    public static U? Map<T, U>(this T? value, Func<T, U> mapper)
        where T : class
        where U : class
    {
        if (value == null) return null;
        return mapper(value);
    }

    public static T OrElseThrow<T, X>(this T? value, Func<X> exceptionSupplier) where X : Exception
    {
        return value ?? throw exceptionSupplier.Invoke();
    }

    public static bool IsNullOrEmpty(this IEnumerable @this)
    {
        if (@this != null)
        {
            return !@this.GetEnumerator().MoveNext();
        }
        return true;
    }

    public static int ConvertToInteger(this string @this)
    {
        if (!int.TryParse(@this, out var result))
        {
            throw new ArgumentException("The string is not a valid integer", @this);
        }
        return result;
    }

    public static string GetIpAddress(HttpContext context)
    {
        var ipAddress = string.Empty;

        var remoteIpAddress = context.Connection.RemoteIpAddress;

        if (remoteIpAddress != null)
        {
            if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
                    .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
            }

            if (remoteIpAddress != null) ipAddress = remoteIpAddress.ToString();

            Console.WriteLine(ipAddress);
            return ipAddress;
        }

        return "127.0.0.1";
    }

    public static string GetIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                Console.WriteLine(ip.ToString());
                return ip.ToString();
            }
        }

        return "127.0.0.1";
    }

    // UtilitiesExtensions.GetLocalIPv4(NetworkInterfaceType.Wireless80211)
    public static string GetLocalIPv4(NetworkInterfaceType _type)
    {
        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        Console.WriteLine(ip.Address.ToString());
                        return ip.Address.ToString();
                    }
                }
            }
        }
        return "127.0.0.1";
    }

    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        Expression<Func<T, bool>> combined = Expression.Lambda<Func<T, bool>>(
            Expression.And(
                left.Body,
                new ExpressionParameterReplacer(right.Parameters, left.Parameters).Visit(right.Body)
            ), left.Parameters);

        return combined;
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        Expression<Func<T, bool>> combined = Expression.Lambda<Func<T, bool>>(
            Expression.Or(
                left.Body,
                new ExpressionParameterReplacer(right.Parameters, left.Parameters).Visit(right.Body)
            ), left.Parameters);

        return combined;
    }

    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        Expression<Func<T, bool>> combined = Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                left.Body,
                new ExpressionParameterReplacer(right.Parameters, left.Parameters).Visit(right.Body)
            ), left.Parameters);

        return combined;
    }

    public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        Expression<Func<T, bool>> combined = Expression.Lambda<Func<T, bool>>(
            Expression.OrElse(
                left.Body,
                new ExpressionParameterReplacer(right.Parameters, left.Parameters).Visit(right.Body)
            ), left.Parameters);

        return combined;
    }

}

public class ExpressionParameterReplacer : ExpressionVisitor
{
    private IDictionary<ParameterExpression, ParameterExpression> ParameterReplacements { get; set; }

    public ExpressionParameterReplacer(
        IList<ParameterExpression> fromParameters,
        IList<ParameterExpression> toParameters)
    {
        ParameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();

        for (int i = 0; i != fromParameters.Count && i != toParameters.Count; i++)
        {
            ParameterReplacements.Add(fromParameters[i], toParameters[i]);
        }
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        ParameterExpression replacement;

        if (ParameterReplacements.TryGetValue(node, out replacement))
        {
            node = replacement;
        }

        return base.VisitParameter(node);
    }
}