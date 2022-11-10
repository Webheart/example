using System.Collections.Generic;
using System.Linq;

public static class ListExtensions
{
    public static void Shuffle< T >( this IList< T > list )
    {
        int n = list.Count;
        while( n > 1 )
        {
            n--;
            int k = rng.Next( n + 1 );
            ( list[ k ], list[ n ] ) = ( list[ n ], list[ k ] );
        }
    }

    static System.Random rng = new System.Random();

    public static List< T > Shuffle< T >( this IEnumerable< T > collection )
    {
        var list = collection.ToList();
        list.Shuffle();
        return list;
    }
}
