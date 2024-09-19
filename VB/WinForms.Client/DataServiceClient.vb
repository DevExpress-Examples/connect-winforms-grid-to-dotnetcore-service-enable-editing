Imports System.Diagnostics
Imports System.Net.Http
Imports System.Text.Json

Namespace WinForms.Client

    Public Module DataServiceClient

        Sub New()
            Dim url As String? = System.Configuration.ConfigurationManager.AppSettings("baseUrl")
            If String.IsNullOrEmpty(url) Then Throw New InvalidOperationException("The 'baseUrl' configuration setting is missing.")
            baseUrl = url
        End Sub

        Private baseUrl As String

        Private Function CreateClient() As HttpClient
            Return New HttpClient()
        End Function

        Public Async Function GetOrderItemsAsync(ByVal skip As Integer, ByVal take As Integer, ByVal sortField As String, ByVal sortAscending As Boolean) As Task(Of DataFetchResult?)
            Dim client = CreateClient()
            Dim response = Await client.GetAsync($"{baseUrl}/data/OrderItems?skip={skip}&take={take}&sortField={sortField}&sortAscending={sortAscending}")
            response.EnsureSuccessStatusCode()
            Dim responseBody = Await response.Content.ReadAsStringAsync()
            Dim dataFetchResult = JsonSerializer.Deserialize(Of DataFetchResult)(responseBody, New JsonSerializerOptions With {.PropertyNameCaseInsensitive = True})
            Return dataFetchResult
        End Function

        <Extension()>
        Private Function AsOrderItem(ByVal responseBody As String) As OrderItem?
            Return JsonSerializer.Deserialize(Of OrderItem)(responseBody, New JsonSerializerOptions With {.PropertyNameCaseInsensitive = True})
        End Function

        Public Async Function GetOrderItemAsync(ByVal id As Integer) As Task(Of OrderItem?)
            Dim client = CreateClient()
            Dim response = Await client.GetAsync($"{baseUrl}/data/OrderItem/{id}")
            response.EnsureSuccessStatusCode()
            Dim responseBody = Await response.Content.ReadAsStringAsync()
            Return responseBody.AsOrderItem()
        End Function

        Public Async Function CreateOrderItemAsync(ByVal orderItem As OrderItem) As Task(Of OrderItem?)
            Dim client = CreateClient()
            Dim response = Await client.PostAsync($"{baseUrl}/data/OrderItem", New StringContent(JsonSerializer.Serialize(orderItem), Encoding.UTF8, "application/json"))
            response.EnsureSuccessStatusCode()
            Dim responseBody = Await response.Content.ReadAsStringAsync()
            Return responseBody.AsOrderItem()
        End Function

        Public Async Function UpdateOrderItemAsync(ByVal orderItem As OrderItem) As Task
            Dim client = CreateClient()
            Dim response = Await client.PutAsync($"{baseUrl}/data/OrderItem/{orderItem.Id}", New StringContent(JsonSerializer.Serialize(orderItem), Encoding.UTF8, "application/json"))
            response.EnsureSuccessStatusCode()
        End Function

        Public Async Function DeleteOrderItemAsync(ByVal id As Integer) As Task(Of Boolean)
            Try
                Dim client = CreateClient()
                Dim response = Await client.DeleteAsync($"{baseUrl}/data/OrderItem/{id}")
                response.EnsureSuccessStatusCode()
                Return True
            Catch ex As Exception
                Debug.WriteLine(ex)
                Return False
            End Try
        End Function
    End Module
End Namespace
