﻿using Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorUI.Client
{
    public class QueryController : ComponentBase
    {
        public Dictionary<string, Func<string,Task>> _etagSubscriptions { get; set; }
        public HubConnectionBuilder _builder { get; set; }
        public HubConnection _connection { get; set; }
        public QueryController(HubConnectionBuilder hub)
        {
            _builder = hub;
            Connect("/hubs/query");
            _etagSubscriptions = new Dictionary<string, Func<string, Task>>();
        }
        public void Connect (string hubUrl)
        {
            _connection = 
                _builder.WithUrl(hubUrl,
                    options =>
                    {
                        options.LogLevel = SignalRLogLevel.Debug;
                        options.Transport = HttpTransportType.WebSockets;
                        options.SkipNegotiation = true;
                    })
                .Build();
            _connection.On <string> ("onChanged", OnChanged);
            _connection.OnClose(close =>
            {
                Debug.WriteLine("The SignalR connection was closed! " + close.ToString());
                return Task.CompletedTask;
            });
            _connection.StartAsync();
            Console.WriteLine("Connection built to SignalR at /hubs/query.");
        }
        public void SubscribeToQuery(string etag, Func<string, Task> handler)
        {
            Console.WriteLine("Entered the SubscribeToQuery method of QueryController.");
            _connection.InvokeAsync("SubscribeToChanged", etag);
            _etagSubscriptions.Add(etag, handler);
        }
        public Task OnChanged(object etag)
        {
            Console.WriteLine("Totem said hello from console.");
            Debug.WriteLine(etag);
            Debug.WriteLine("Totem said hello from debug!");
            var checkpointIndex = etag.ToString().IndexOf("@");
            var subscription = SanitizeETag(etag.ToString(), checkpointIndex);
            Debug.WriteLine("Found a handler to callback for this subscription: " + subscription);

            return _etagSubscriptions.First(sub => sub.Key == subscription).Value.Invoke(subscription.ToString());
        }
        public string SanitizeETag(string etag, int etagCheckpoint)
        {
            var span = etag.AsSpan();
            var builder = new StringBuilder();
            for (int i = 0; i < span.Length; i++)
            {
                if (i < etagCheckpoint)
                    builder.Append(span[i]);
            }
            return builder.ToString();
        }
        public string Test()
        {
            return _connection.ToString();
        }
    }
}
