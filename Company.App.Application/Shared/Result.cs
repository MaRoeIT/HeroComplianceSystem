namespace Company.App.Application.Shared
{
    /// <summary>
    /// Represents the outcome of an operation, containing either a successful result value or an error message.
    /// </summary>
    /// <remarks>Use this type to encapsulate the result of an operation that can either succeed with a value
    /// or fail with an error message. When <paramref name="IsSuccess"/> is <see langword="true"/>, <paramref
    /// name="Value"/> contains the result and <paramref name="Error"/> is empty. When <paramref name="IsSuccess"/> is
    /// <see langword="false"/>, <paramref name="Error"/> contains the error message and <paramref name="Value"/> is
    /// typically ignored.</remarks>
    /// <typeparam name="T">The type of the value returned if the operation is successful.</typeparam>
    /// <param name="Value">The value produced by the operation if it was successful; otherwise, the default value for the type.</param>
    /// <param name="IsSuccess">A value indicating whether the operation was successful.</param>
    /// <param name="Error">The error message describing the failure if the operation was not successful. This value is an empty string if
    /// the operation succeeded.</param>
    public record Result<T>(T? Value, bool IsSuccess, string Error = "");

    /// <summary>
    /// Represents the outcome of an operation, indicating success or failure and providing an associated error message
    /// if applicable.
    /// </summary>
    /// <remarks>Use this record to standardize the reporting of operation results, especially in scenarios
    /// where only success status and an optional error message are needed. The <paramref name="Error"/> property should
    /// be set only when <paramref name="IsSuccess"/> is <see langword="false"/>.</remarks>
    /// <param name="IsSuccess">A value indicating whether the operation was successful. Set to <see langword="true"/> if the operation
    /// succeeded; otherwise, <see langword="false"/>.</param>
    /// <param name="Error">The error message associated with a failed operation. This value is typically empty if <paramref
    /// name="IsSuccess"/> is <see langword="true"/>.</param>
    public record Result(bool IsSuccess, string Error = "");
}
