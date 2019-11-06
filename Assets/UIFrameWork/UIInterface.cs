using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

namespace UIFrameWork
{
    public interface IGameObject {
        void SetObjectActive(bool active);
    }

    public interface IText {
        void SetTextText(string text);
        string GetTextText();
    }
    public interface IImage {
        
    }
    public interface IButton {
        void AddOnClickListioner(UnityAction action);
        void ClearListioner();
    }
    public interface IInputField {
        string GetInputText();
        void SetInputText(string text);
        void AddOnValueChange();
    }

    public class UIMonoBehaviours: MonoBehaviour, IText, IImage, IButton, IInputField, IGameObject{

        // 获取所有组件
        protected Text m_text;
        protected Image m_image;
        protected InputField m_input;
        protected Button m_button;

        protected void InitUIComponents(){
            m_text = GetComponent<Text>();
            m_image = GetComponent<Image>();
            m_input = GetComponent<InputField>();
            m_button = GetComponent<Button>();
        }




        public virtual void SetObjectActive(bool active){
            gameObject.SetActive(active);
        }
        public virtual void SetTextText(string text){
            if(m_text != null){
                m_text.text = text; 
            }
        }
        public virtual string GetTextText(){
            if(m_text != null){
                return m_text.text; 
            }
            return null;
        }
        public virtual void AddOnClickListioner(UnityAction action){
            if(m_button != null){
                m_button.onClick.AddListener(action);
            }
        }
        public virtual void AddOnClickListioner(UnityAction<int> action){}
        public virtual void ClearListioner(){}
        public virtual string GetInputText(){return null;}
        public virtual void SetInputText(string text){}
        public virtual void AddOnValueChange(){}
    }
}