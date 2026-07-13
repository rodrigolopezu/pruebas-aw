using Cortex.Mediator.Notifications;

namespace eb17953u202421866.Shared.Domain.Model.Events;

/// <summary>
///     Represents a domain/integration event in the system.
/// </summary>
/// <remarks>
///     This interface marks classes as events that can be published and handled by the mediator.
///     It extends <see cref="INotification" /> to integrate with Cortex.Mediator's notification pipeline.
///     In a monolith with multiple bounded contexts, the event type itself should live in Shared
///     (see technical constraints for exams that require integration events, e.g. FolderRegisteredEvent),
///     while the emitting bounded context publishes it and the reacting bounded context implements the handler.
///     Author: __YOUR_NAME__
/// </remarks>
public interface IEvent : INotification
{
}
