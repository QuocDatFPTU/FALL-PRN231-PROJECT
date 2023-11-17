using System.Collections;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace HotelBooking.Application.Helpers;
public static class UtilitiesExtensions
{
    public static T OrElseThrow<T, X>(this T? value, Func<X> exceptionSupplier) where X : Exception
    {
        return value ?? throw exceptionSupplier.Invoke();
    }

    public static bool IsNullOrEmpty(this IEnumerable? @this)
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

    public static T DeepClone<T>(this T obj)
    {
        var jsonString = JsonSerializer.Serialize(obj);
        return JsonSerializer.Deserialize<T>(jsonString);
    }

    public static bool RemoveIf<T>(this ICollection<T> collection, Func<T, bool> filter)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));
        bool removed = false;
        var itemsToRemove = collection.Where(filter).ToList();
        foreach (var item in itemsToRemove)
        {
            collection.Remove(item);
            removed = true;
        }
        return removed;
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