# TinyHttpClientPool
A HttpClient Pool that allows for reuse of http clients by assuring that you have the full and only access to an HttpClient during your use of it.

It is Tiny and only a few classes big.

## Build status

![Build status](https://io2gamelabs.visualstudio.com/_apis/public/build/definitions/be16d002-5786-41a1-bf3b-3e13d5e80aa0/13/badge)

## Why use it?

* Exclusive access to an HttpClient while calling
* Allows for custom headers per request while reusing HttpClients
* Pools are cool

## Why is it created

It's created with Xamarin in mind and that's where we use it.

## Usage

### Getting an instance of the pool manager

There are a few ways to get a hold of an instance.

```csharp
// A) Just create a new instance
var pool = new TinyHttpClientPool();

// B) Use the static helpers
TinyHttpClientPool.Initialize();
var client = TinyHttpClientPool.FetchClient();

// C) Register the interface into your favorite IoC container
var pool = Resolver.Resolve<ITinyHttpClientPool>();
```

The usage from this point on is pretty straight forward

```csharp
// Create an instance in any way above
var pool = new TinyHttpClientPool();

// Always use a using around the client!
using (HttpClient client = pool.Fetch())
{
    // As long as you are in the using, the instance
    // is yours and yours alone
    var result = await client.GetStringAsync(url);
}

// At this point it is returned to the pool
```

## Initialization
> Note: We are currently working on a better way to initialize the pool. But for the time being, you can hook up to the ```ClientInitialization``` action and configure each new ```HttpClient``` with default values.
>
> What we are thinking of is a configuration object passed into the constructor of the Initialize call and/or the constructor of the TinyHttpClientPool class.

For example, setting a common base url on each new ```HttpClient```is done like this:
```csharp
 TinyHttpClientPool.Current.ClientInitializationOnCreation = (obj) => obj.BaseAddress = new Uri(BackendUrl);
```

You can also run common code on each fetch.
```csharp
TinyHttpClientPool.Current.ClientInitializationOnFetch = (obj) => ChangeHeaders(obj);
```

## Monitoring

You can monitor the pool size and number of available ```HttpClients``` and hook up to the ```PoolChanged``` event to be notified if something changes.

```csharp
TinyHttpClientPool.Current.PoolChanged += (sender, e) =>
{
    PoolSize = TinyHttpClientPool.Current.TotalPoolSize;
    Available = TinyHttpClientPool.Current.AvailableCount;
}
```

## Cleanup

If you want to flush the pool, simply call ```Flush()```

```csharp
pool.Flush();
```

This will dispose any ```HttpClient``` that is in the ```Available``` state but not touch those who are in the ```InUse``` state.

## Important

For this to work, you must dispose the client when you are done with it. There is no other way to return it to the pool. Well, there is one way...

The returned client isn't really an ```HttpClient```, but rather a ```TinyHttpClient``` that inherits from ```HttpClient```.

So you could manually set the state to ```State.Available``` but that is at your own risk.

```csharp
var client = pool.Fetch() as TinyHttpClient;

// do stuff with client here

// Mark it as ready for the pool to reuse
client.State = State.Available; 
```


## Roadmap

There isn't really any roadmap to this since it's pretty much done the way it's supposed to work. But if you're missing a feature, create an issue or better yet, create a PR. :)