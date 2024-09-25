Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid

Namespace WinForms.Client

    Partial Public Class MainForm
        Inherits XtraForm

        Public Sub New()
            InitializeComponent()
        End Sub

        Private loader As VirtualServerModeDataLoader = Nothing

        Private Sub VirtualServerModeSource_ConfigurationChanged(sender As Object, e As DevExpress.Data.VirtualServerModeRowsEventArgs)
            loader = New VirtualServerModeDataLoader(e.ConfigurationInfo)
            e.RowsTask = loader.GetRowsAsync(e)
        End Sub

        Private Sub VirtualServerModeSource_MoreRows(sender As Object, e As DevExpress.Data.VirtualServerModeRowsEventArgs)
            If loader IsNot Nothing Then
                e.RowsTask = loader.GetRowsAsync(e)
            End If
        End Sub

        Private Async Sub gridView1_DoubleClick(sender As Object, e As EventArgs)
            If TypeOf sender Is GridView Then
                Dim view As GridView = DirectCast(sender, GridView)
                If TypeOf view.FocusedRowObject Is OrderItem Then
                    Dim oi As OrderItem = DirectCast(view.FocusedRowObject, OrderItem)
                    Dim editResult = EditForm.EditItem(oi)
                    If editResult.changesSaved Then
                        Await DataServiceClient.UpdateOrderItemAsync(editResult.item)
                        view.RefreshData()
                    End If
                End If
            End If
        End Sub

        Private Async Sub addItemButton_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
            If TypeOf gridControl.FocusedView Is ColumnView Then
                Dim view As ColumnView = DirectCast(gridControl.FocusedView, ColumnView)
                Dim createResult = EditForm.CreateItem()
                If createResult.changesSaved Then
                    Await DataServiceClient.CreateOrderItemAsync(createResult.item)
                    view.RefreshData()
                End If
            End If
        End Sub

        Private Async Sub deleteItemButton_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
            If TypeOf gridControl.FocusedView Is ColumnView Then
                Dim view As ColumnView = DirectCast(gridControl.FocusedView, ColumnView)
                Dim orderItem As OrderItem = TryCast(view.GetFocusedRow(), OrderItem)
                If orderItem IsNot Nothing Then
                    Await DataServiceClient.DeleteOrderItemAsync(orderItem.Id)
                    view.RefreshData()
                End If
            End If
        End Sub
    End Class


End Namespace
