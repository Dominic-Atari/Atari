// See https://aka.ms/new-console-template for more information

var publisher = new Publisher();
var subscriber = new Subscriber();

subscriber.SubscribeOnMyEvent(publisher); // Subscribe to the event
publisher.DoSomething("10 AM", publisher); // Pass the publisher instance