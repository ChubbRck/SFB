using UnityEngine;
#region Header
/**
 *
 * original version available in https://github.com/anchan828/property-drawer-collection
 * 
**/
#endregion
namespace com.FDT.EditorTools
{
	public class PopupAttribute : PropertyAttribute
	{
	    public object[] list;
	    
	    public PopupAttribute (params object[] list)
	    {
	        this.list = list;
	    }
	}
}