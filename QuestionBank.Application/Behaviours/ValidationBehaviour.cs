using FluentValidation;
using MediatR;

namespace QuestionBank.Application.Behaviors;

/// <summary>
/// A MediatR pipeline behavior that applies FluentValidation validators to incoming requests.
/// If any validation rules fail, a <see cref="FluentValidation.ValidationException"/> is thrown
/// before the request reaches its handler.
/// </summary>
/// <typeparam name="TRequest">The type of the incoming request.</typeparam>
/// <typeparam name="TResponse">The type of the expected response.</typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// The collection of FluentValidation validators that apply to the current request type.
    /// </summary>
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="validators">A collection of FluentValidation validators for the request type.</param>
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <summary>
    /// Executes the validation behavior in the MediatR pipeline.
    /// If validation succeeds, the next handler is called. If validation fails,
    /// a <see cref="FluentValidation.ValidationException"/> is thrown.
    /// </summary>
    /// <param name="request">The request instance to be validated.</param>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The response from the next handler in the pipeline.</returns>
    /// <exception cref="FluentValidation.ValidationException">
    /// Thrown if one or more validators produce validation failures.
    /// </exception>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Count != 0)
                throw new FluentValidation.ValidationException(failures);
        }
        return await next();
    }
}
