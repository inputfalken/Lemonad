# 🍋 Lemonad 🍋

## Status

### WIP (A lot may change)

There's currently no nuget package available to download this.

## Summary

A functional **C#** library with data structures and functions whose main goal
is to have an [declarative](https://en.wikipedia.org/wiki/Declarative_programming)
approach to problem solving by using various
[HOF's](https://en.wikipedia.org/wiki/Higher-order_function#C#)
to achieve cleaner code bases & code which is easier to think about.

For example the `Either<TLeft, TRight>` type,
which enables you to skip using exceptions for error handling.
The way `Either<TLeft, TRight>` works is that there's
always one value present. **Either** it's the
value to the **left** or the value to the **right**.
In this library, the value to the right is considered **successfull**
and the left value is considered to be a **failure**.

The following example illustrates how you can perform a division
and return a string with an error message rather
than throwing an exception using a similiar message.

```csharp

Either<string, int> Divide(int x, int y) {
    if (y != 0)
        return  x / y;
    else
        return "Cannot divide with zero";
}


List<Either<string, int>> eithers = new List<Either<string, int>> {
  Divide(4, 2),
  Divide(3, 0),
  Divide(3, 3),
  Divide(4, 0),
  Divide(5, 0),
  Divide(6, 0),
  Divide(7, 0),
  Divide(8, 0),
  Divide(10, 2)
};
List<int> successFulDivisions = eithers.EitherRights().ToList();
IEnumerable<string> failedDivisions = eithers.EitherLefts();

// Prints all the numbers where the 'y' parameter from the function is not 0.
// 2
// 1
// 5
foreach (int division in successFulDivisions) { Console.WriteLine(division); }

// Prints all the messages where the 'y' parameter from the function is 0.
// Cannot divide with zero
// Cannot divide with zero
// …
foreach (int message in failedDivisions) { Console.WriteLine(message); }


```

Or if you do not care about handling a **failure** type,
you could use the `Maybe<T>` type. The maybe type works
just like `Either<TLeft, TRight>` except that there's
no **failure** (TLeft) type available.
It's works exactly as `Nullable<T>` (aka `T?`)
does except that you can use this with reference
types (`string`, `object`…) as well.

The following example illustrates how you can perform a division
and instead of throwing an exception; you could return `Maybe<T>.None`

``` csharp
Maybe<int> Divide(int x, int y) {
    if (y != 0)
        return x / y;
    else {
        return Maybe<int>.None;
    }
}

List<Maybe<int>> maybes = new List<Maybe<int>> {
    Divide(4, 2),
    Divide(3, 0),
    Divide(3, 3),
    Divide(4, 0),
    Divide(5, 0),
    Divide(6, 0),
    Divide(7, 0),
    Divide(8, 0),
    Divide(10, 2)
};

List<int> successFulDivisions = maybes.MaybeSome().ToList();

// Prints all the numbers where the 'y' parameter from the function is not 0.
// 2
// 1
// 5
foreach (int division in successFulDivisions) { Console.WriteLine(division); }


```

## Current supported data types

* ErrorHandling
  * `Either<TLeft, TRight>`
  * `Maybe<T>`
