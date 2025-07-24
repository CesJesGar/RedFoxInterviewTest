namespace RedFox.Application.Service.Api
{
    public interface IUserService
    {
        /// <summary>
        ///     Retrieves user data from the remote API as a stream
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to abort the operation</param>
        /// <returns>
        ///     A <see cref="Stream"/> containing the response body in JSON format
        /// </returns>
        /// <exception cref="HttpRequestException">
        ///     Thrown when the HTTP response status code does not indicate success
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     Thrown when the operation is canceled via the cancellation token
        /// </exception>
        Task<Stream> GetUsers(CancellationToken cancellationToken);

        /// <summary>
        ///     Retrieves a single user by <paramref name="id"/> from the remote API as a stream
        /// </summary>
        /// <param name="id">Identifier of the user to fetch</param>
        /// <param name="cancellationToken">Cancellation token to abort the operation</param>
        /// <returns>
        ///     A <see cref="Stream"/> containing the response body in JSON format
        /// </returns>
        /// <exception cref="HttpRequestException">
        ///     Thrown when the HTTP response status code does not indicate success
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     Thrown when the operation is canceled via the cancellation token
        /// </exception>
        Task<Stream> GetUserById(int id, CancellationToken cancellationToken);

        /// <summary>
        ///     Creates a new user by posting the supplied JSON stream to the remote API
        /// </summary>
        /// <param name="userJson">A <see cref="Stream"/> containing the JSON body of the new user</param>
        /// <param name="cancellationToken">Cancellation token to abort the operation</param>
        /// <returns>
        ///     A <see cref="Stream"/> containing the response body (typically the created user) in JSON format
        /// </returns>
        /// <exception cref="HttpRequestException">
        ///     Thrown when the HTTP response status code does not indicate success
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     Thrown when the operation is canceled via the cancellation token
        /// </exception>
        Task<Stream> CreateUser(Stream userJson, CancellationToken cancellationToken);

        /// <summary>
        ///     Updates an existing user identified by <paramref name="id"/> by sending the supplied JSON stream to the remote API
        /// </summary>
        /// <param name="id">Identifier of the user to update</param>
        /// <param name="userJson">A <see cref="Stream"/> containing the JSON body with updated user data</param>
        /// <param name="cancellationToken">Cancellation token to abort the operation</param>
        /// <returns>
        ///     A <see cref="Stream"/> containing the response body (typically the updated user) in JSON format
        /// </returns>
        /// <exception cref="HttpRequestException">
        ///     Thrown when the HTTP response status code does not indicate success
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     Thrown when the operation is canceled via the cancellation token
        /// </exception>
        Task<Stream> UpdateUser(int id, Stream userJson, CancellationToken cancellationToken);

        /// <summary>
        ///     Deletes the user identified by <paramref name="id"/> from the remote API
        /// </summary>
        /// <param name="id">Identifier of the user to delete</param>
        /// <param name="cancellationToken">Cancellation token to abort the operation</param>
        /// <returns>
        ///     A <see cref="Task"/> that completes when the delete operation finishes
        /// </returns>
        /// <exception cref="HttpRequestException">
        ///     Thrown when the HTTP response status code does not indicate success
        /// </exception>
        /// <exception cref="OperationCanceledException">
        ///     Thrown when the operation is canceled via the cancellation token
        /// </exception>
        Task DeleteUser(int id, CancellationToken cancellationToken);
    }
}
