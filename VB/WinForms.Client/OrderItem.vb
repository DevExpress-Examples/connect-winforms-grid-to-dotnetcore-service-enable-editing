Imports DevExpress.CodeParser
Namespace WinForms.Client
	Public Class OrderItem
		Public Property Id() As Integer
		Public Property ProductName() As String = Nothing

		Public Property UnitPrice() As Decimal
		Public Property Quantity() As Integer
		Public Property Discount() As Single
		Public Property OrderDate() As Date
	End Class
End Namespace
