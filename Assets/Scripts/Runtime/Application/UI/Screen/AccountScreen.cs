using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class AccountScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private TMP_InputField _nameField;
        [SerializeField] private TMP_InputField _ageField;
        [SerializeField] private TMP_InputField _genderField;

        public event Action OnBackPressed;
        public event Action OnSavePressed;

        public event Action<string> OnNameChanged;
        public event Action<string> OnAgeChanged;
        public event Action<string> OnGenderChanged;

        private void OnDestroy()
        {
            _backButton.onClick.RemoveAllListeners();
            _saveButton.onClick.RemoveAllListeners();

            _nameField.onEndEdit.RemoveAllListeners();
            _ageField.onEndEdit.RemoveAllListeners();
            _genderField.onEndEdit.RemoveAllListeners();
        }

        public void Initialize(UserProfileData data)
        {
            SubscribeToEvents();
            SetData(data);
        }

        public void SetData(UserProfileData data)
        {
            _nameField.text = data.Username;
            _ageField.text = data.Age.ToString();
            _genderField.text = data.Gender;
        }

        private void SubscribeToEvents()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
            _saveButton.onClick.AddListener(() => OnSavePressed?.Invoke());

            _nameField.onEndEdit.AddListener((value) => OnNameChanged?.Invoke(value));
            _ageField.onEndEdit.AddListener((value) => OnAgeChanged?.Invoke(value));
            _genderField.onEndEdit.AddListener((value) => OnGenderChanged?.Invoke(value));
        }
    }
}