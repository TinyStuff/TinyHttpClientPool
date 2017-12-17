using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Models;
using TinyHttpClientPoolLib;

namespace Sample.Services
{
    public class TodoService
    {
        public async Task<IEnumerable<TodoItem>> GetTodoItems(bool slow = false)
        {
            using(var client = TinyHttpClientPool.FetchClient())
            {
                if (slow)
                {
					await Task.Delay(3000);
                }

                var url = "/todos";
                var json = await client.GetStringAsync(url);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<TodoItem>>(json); 
            }
        }
    }
}
