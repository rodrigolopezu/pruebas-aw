using Cortex.Mediator.Notifications;
using eb17953u202421866.Shared.Domain.Model.Events;

namespace eb17953u202421866.Shared.Application.Internal.EventHandlers;

/// <summary>
///     Base interface for event handlers reacting to a specific <see cref="IEvent" />.
/// </summary>
/// <remarks>
///     Implement this in the bounded context that needs to REACT to the event, not necessarily
///     the one that emits it. Register no additional DI for this: Cortex.Mediator's
///     AddCortexMediator scans the assembly and wires INotificationHandler implementations automatically.
///     Author: __YOUR_NAME__
/// </remarks>
/// <typeparam name="TEvent">The event type this handler reacts to.</typeparam>
public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
}
