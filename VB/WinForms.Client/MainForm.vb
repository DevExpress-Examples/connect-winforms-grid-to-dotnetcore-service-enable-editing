Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid

Namespace WinForms.Client

    Public Partial Class MainForm
        Inherits XtraForm

        Public Sub New()
            InitializeComponent()
        End Sub

        Private loader As VirtualServerModeDataLoader?

        Private Sub VirtualServerModeSource_ConfigurationChanged(ByVal sender As Object?, ByVal e As DevExpress.Data.VirtualServerModeRowsEventArgs)
            loader = New VirtualServerModeDataLoader(e.ConfigurationInfo)
            e.RowsTask = loader.GetRowsAsync(e)
        End Sub

        Private Sub VirtualServerModeSource_MoreRows(ByVal sender As Object?, ByVal e As DevExpress.Data.VirtualServerModeRowsEventArgs)
            If TypeOf loader Is [not] Then Nothing
            If True Then
                e.RowsTask = loader.GetRowsAsync(e)
            End If
        End Sub

        Private Async Sub gridView1_DoubleClick(ByVal sender As Object, ByVal e As EventArgs)
            Dim view As GridView = Nothing, oi As OrderItem = Nothing
            If CSharpImpl.__Assign(view, TryCast(sender, GridView)) IsNot Nothing Then
                If CSharpImpl.__Assign(oi, TryCast(view.FocusedRowObject, OrderItem)) IsNot Nothing Then
                    Dim editResult = EditForm.EditItem(oi)
                    If editResult.changesSaved Then
                        Await DataServiceClient.UpdateOrderItemAsync(editResult.item)
                        view.RefreshData()
                    End If
                End If
            End If
        End Sub

        Private Async Sub addItemButton_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
            Dim view As ColumnView = Nothing
            If CSharpImpl.__Assign(view, TryCast(gridControl.FocusedView, ColumnView)) IsNot Nothing Then
                Dim createResult = EditForm.CreateItem()
                If createResult.changesSaved Then
                     ''' Cannot convert AwaitExpressionSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitAwaitExpression(AwaitExpressionSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
''' 
''' Input:
'''                     await WinForms.Client.DataServiceClient.CreateOrderItemAsync(createResult.item!)
'''  view.RefreshData()
                End If
            End If
        End Sub

        Private Async Sub deleteItemButton_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
            Dim view As ColumnView = Nothing, orderItem As OrderItem = Nothing
            If CSharpImpl.__Assign(view, TryCast(gridControl.FocusedView, ColumnView)) IsNot Nothing AndAlso CSharpImpl.__Assign(orderItem, TryCast(view.GetFocusedRow(), OrderItem)) IsNot Nothing Then
                Await DataServiceClient.DeleteOrderItemAsync(orderItem.Id)
                view.RefreshData()
            End If
        End Sub

        Private Class CSharpImpl

            <System.Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
            Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
                target = value
                Return value
            End Function
        End Class
    End Class
End Namespace
