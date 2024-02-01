using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
    public class CloseWindowButton : MonoBehaviour
    {
        private IWindow _window;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _window = GetComponentInParent<IWindow>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(_window.Hide);
        }
        
        private void OnDisable()
        {
            _button.onClick.RemoveListener(_window.Hide);
        }
    }
