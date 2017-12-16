# TinyHttpClientPool
A HttpClient Pool that allows for reuse of http clients by assuring that you have the full and only access to an HttpClient during your use of it.

It is Tiny and only a few classes big.

## Why use it?

* Exclusive access to an HttpClient while calling
* Allows for custom headers
* Reuse of already instansiated HttpClients

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