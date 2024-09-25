Imports System.Diagnostics
Imports System.Net.Http
Imports System.Text
Imports System.Text.Json

Namespace WinForms.Client

    Public NotInheritable Class DataServiceClient
        Private Shared ReadOnly baseUrl As String

        Shared Sub New()
            Dim url As String = System.Configuration.ConfigurationManager.AppSettings("baseUrl")
            If String.IsNullOrEmpty(url) Then
                Throw New InvalidOperationException("The 'baseUrl' configuration setting is missing.")
            End If
            baseUrl = url
        End Sub

        Private Shared Function CreateClient() As HttpClient
            Return New HttpClient()
        End Function

        Public Shared Async Function GetOrderItemsAsync(skip As Integer, take As Integer, sortField As String, sortAscending As Boolean) As Task(Of DataFetchResult)
            Using client As HttpClient = CreateClient()
                Dim response As HttpResponseMessage = Await client.GetAsync($"{baseUrl}/data/OrderItems?skip={skip}&take={take}&sortField={sortField}&sortAscending={sortAscending}")
                response.EnsureSuccessStatusCode()
                Dim responseBody As String = Await response.Content.ReadAsStringAsync()

                Dim dataFetchResult As DataFetchResult = JsonSerializer.Deserialize(Of DataFetchResult)(responseBody, New JsonSerializerOptions() With {
                .PropertyNameCaseInsensitive = True
            })
                Return dataFetchResult
            End Using
        End Function

        Private Shared Function AsOrderItem(responseBody As String) As OrderItem
            Return JsonSerializer.Deserialize(Of OrderItem)(responseBody, New JsonSerializerOptions() With {
            .PropertyNameCaseInsensitive = True
        })
        End Function

        Public Shared Async Function GetOrderItemAsync(id As Integer) As Task(Of OrderItem)
            Using client As HttpClient = CreateClient()
                Dim response As HttpResponseMessage = Await client.GetAsync($"{baseUrl}/data/OrderItem/{id}")
                response.EnsureSuccessStatusCode()
                Dim responseBody As String = Await response.Content.ReadAsStringAsync()
                Return AsOrderItem(responseBody)
            End Using
        End Function

        Public Shared Async Function CreateOrderItemAsync(orderItem As OrderItem) As Task(Of OrderItem)
            Using client As HttpClient = CreateClient()
                Dim response As HttpResponseMessage = Await client.PostAsync($"{baseUrl}/data/OrderItem", New StringContent(JsonSerializer.Serialize(orderItem), Encoding.UTF8, "application/json"))
                response.EnsureSuccessStatusCode()
                Dim responseBody As String = Await response.Content.ReadAsStringAsync()
                Return AsOrderItem(responseBody)
            End Using
        End Function

        Public Shared Async Function UpdateOrderItemAsync(orderItem As OrderItem) As Task
            Using client As HttpClient = CreateClient()
                Dim response As HttpResponseMessage = Await client.PutAsync($"{baseUrl}/data/OrderItem/{orderItem.Id}", New StringContent(JsonSerializer.Serialize(orderItem), Encoding.UTF8, "application/json"))
                response.EnsureSuccessStatusCode()
            End Using
        End Function

        Public Shared Async Function DeleteOrderItemAsync(id As Integer) As Task(Of Boolean)
            Try
                Using client As HttpClient = CreateClient()
                    Dim response As HttpResponseMessage = Await client.DeleteAsync($"{baseUrl}/data/OrderItem/{id}")
                    response.EnsureSuccessStatusCode()
                    Return True
                End Using
            Catch ex As Exception
                Debug.WriteLine(ex)
                Return False
            End Try
        End Function
    End Class

End Namespace
