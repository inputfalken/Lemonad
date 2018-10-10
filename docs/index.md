# Lemonad

## Description

A functional C# library with data structures and functions whose main goal is to have an declarative approach to problem solving by using various HOF's to achieve cleaner code bases & code which is easier to think about.

For example the Result<T, TError> type, which enables you to skip using exceptions for error handling. The way Result<T, TError> works is that there's always one value present. Either it's the value to the left or the value to the right (TError). In this library, the value to the Left is considered successfull and the right value is considered to be a failure.

The following example illustrates how you can perform a division and return a string with an error message rather than throwing an exception using a similiar message.

## How to install

### NuGet Package Manager:

```PowerShell
PM> Install-Package Lemonad.ErrorHandling
```

### .NET CLI:

```PowerShell
> dotnet add package Lemonad.ErrorHandling
 ```

### Examples

Look at [samples](https://github.com/inputfalken/Lemonad/tree/master/samples) in GitHub for some concrete examples.
