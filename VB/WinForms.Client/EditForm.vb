Imports System.Windows.Forms
Namespace WinForms.Client
    Partial Public Class EditForm
        Inherits DevExpress.XtraEditors.XtraForm

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Shared Function EditItem(orderItem As OrderItem) As (changesSaved As Boolean, item As OrderItem)
            Using form As New EditForm()
                Dim clonedItem As New OrderItem() With {
                .Id = orderItem.Id,
                .ProductName = orderItem.ProductName,
                .UnitPrice = orderItem.UnitPrice,
                .Quantity = orderItem.Quantity,
                .Discount = orderItem.Discount,
                .OrderDate = orderItem.OrderDate
            }
                form.orderItemBindingSource.DataSource = clonedItem
                If form.ShowDialog() = DialogResult.OK Then
                    Return (True, clonedItem)
                End If
                Return (False, orderItem)
            End Using
        End Function

        Public Shared Function CreateItem() As (changesSaved As Boolean, item As OrderItem)
            Using form As New EditForm()
                Dim newItem As New OrderItem() With {
                .OrderDate = DateTime.Now
            }
                form.orderItemBindingSource.DataSource = newItem
                If form.ShowDialog() = DialogResult.OK Then
                    Return (True, newItem)
                End If
                Return (False, Nothing)
            End Using
        End Function
    End Class

End Namespace
