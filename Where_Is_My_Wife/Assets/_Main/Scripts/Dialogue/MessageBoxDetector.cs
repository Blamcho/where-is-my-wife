using UnityEngine;

namespace WhereIsMyWife.Controllers
{
    public class MessageBoxDetector : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var trigger = other.gameObject.GetComponent<MessageBoxTrigger>();
            if (trigger != null)
            {
                MessageBoxController.Instance.AppearBubble(trigger.Text, trigger.ButtonType);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var trigger = other.gameObject.GetComponent<MessageBoxTrigger>();
            if (trigger != null)
            {
                MessageBoxController.Instance.DissappearBubble();
            }
        }
    }
}

