using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;

namespace System.Net.Http;

public static class HttpClientExtension
{
    public static async Task InvokeAsync(this HttpClient httpClient, HttpMethod httpMethod, string methodName)
    {
        var httpRequestMessage = new HttpRequestMessage(httpMethod, methodName);

        var response = await httpClient.SendAsync(httpRequestMessage);

        await ProcessResponseAsync(response);
    }

    public static async Task InvokeAsync<TRequest>(this HttpClient httpClient, HttpMethod httpMethod, string methodName, TRequest data)
    {
        var httpRequestMessage = new HttpRequestMessage(httpMethod, methodName)
        {
            Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
        };

        var response = await httpClient.SendAsync(httpRequestMessage);

        await ProcessResponseAsync(response);
    }

    public static async Task<TValue> InvokeAsync<TValue>(this HttpClient httpClient, HttpMethod httpMethod, string methodName)
    {
        var httpRequestMessage = new HttpRequestMessage(httpMethod, methodName);

        var response = await httpClient.SendAsync(httpRequestMessage);

        return await ProcessResponseAsync<TValue>(response);
    }

    public static async Task<TValue> InvokeAsync<TRequest, TValue>(this HttpClient httpClient, HttpMethod httpMethod, string methodName, TRequest data)
    {
        var httpRequestMessage = new HttpRequestMessage(httpMethod, methodName)
        {
            Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
        };

        var response = await httpClient.SendAsync(httpRequestMessage);

        return await ProcessResponseAsync<TValue>(response);
    }

    private static async Task<TResponse> ProcessResponseAsync<TResponse>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Accepted:
                case HttpStatusCode.NoContent:
                    return default(TResponse);
                case (HttpStatusCode)299:
                    throw new Exception(await response.Content.ReadAsStringAsync());
                default:
                    return await response.Content.ReadFromJsonAsync<TResponse>();
            }
        }
        else
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
    }

    private static async Task ProcessResponseAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case (HttpStatusCode)299:
                    throw new Exception(await response.Content.ReadAsStringAsync());
                default:
                    break;
            }
        }
        else if ((response.Content.Headers.ContentLength ?? 0) > 0)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
        else
        {
            throw new Exception($"StatusCode: {response.StatusCode}");
        }
    }
}