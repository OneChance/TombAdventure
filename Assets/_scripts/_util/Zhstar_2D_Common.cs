using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zhstar_2D_Common : MonoBehaviour
{

	public static Vector3 checkPosValid (Vector3 checkedPos, List<string> checkedTags, float itemWidth, float itemHeight)
	{

		//check the pos is valid,there is no player,no ground item
		bool valid = true;

		RaycastHit2D hitLeft = Physics2D.Raycast (new Vector2 (checkedPos.x, checkedPos.y), Vector2.left, itemWidth / 2);
		RaycastHit2D hitRight = Physics2D.Raycast (new Vector2 (checkedPos.x, checkedPos.y), Vector2.right, itemWidth / 2);
		RaycastHit2D hitUp = Physics2D.Raycast (new Vector2 (checkedPos.x, checkedPos.y), Vector2.up, itemHeight / 2);
		RaycastHit2D hitDown = Physics2D.Raycast (new Vector2 (checkedPos.x, checkedPos.y), Vector2.down, itemHeight / 2);


		if (hitLeft.collider != null && checkedTags.Contains (hitLeft.collider.gameObject.tag)) {
			valid = false;
		}

		if (hitRight.collider != null && checkedTags.Contains (hitRight.collider.gameObject.tag)) {
			valid = false;
		}

		if (hitUp.collider != null && checkedTags.Contains (hitUp.collider.gameObject.tag)) {
			valid = false;
		}

		if (hitDown.collider != null && checkedTags.Contains (hitDown.collider.gameObject.tag)) {
			valid = false;
		}
		
		if (!valid) {
			return new Vector3 (-999, -999, -999);
		}
		
		return checkedPos;
	}
}
