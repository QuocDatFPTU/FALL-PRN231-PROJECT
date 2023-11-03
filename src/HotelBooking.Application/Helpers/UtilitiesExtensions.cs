using System.Collections;

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
}
