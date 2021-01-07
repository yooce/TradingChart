namespace MagicalNuts.DataTypes
{
	/// <summary>
	/// プロパティ利用者のインターフェースを表します。
	/// </summary>
	public interface IPropertyUser
	{
		/// <summary>
		/// プロパティ名を取得します。
		/// </summary>
		string Name { get; }

		/// <summary>
		/// プロパティを取得します。
		/// </summary>
		object Properties { get; }
	}
}
