// ========================================================
// 描述：Demo 02 —— 通过继承 + 接口实现 图片拖动替换位置。
// 作者：Chinar 
// 创建时间：2019-04-29 16:39:11
// 版 本：1.0
// ========================================================
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



namespace QmDreamer.UI
{
    /// <summary>
    /// 管理UI元素排序：使UI可通过拖动进行位置互换
    /// </summary>
    public class ChinarDragSwapImage : Button, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Transform beginParentTransform; //记录开始拖动时的父级对象        
        /// <summary>
        /// UI界面的顶层，这里我用的是 Canvas
        /// (这个变量在开发中设置到单例中较好，不然每一个物品都会初始化查找
        /// GameObject.Find("Canvas").transform;)
        /// </summary>
        private Transform topOfUiT;


        protected override void Start()
        {
            base.Start();
            topOfUiT = GameObject.Find("Canvas").transform;
        }


        public void OnBeginDrag(PointerEventData _)
        {
            if (transform.parent == topOfUiT) return;
            beginParentTransform = transform.parent;
            transform.SetParent(topOfUiT);
        }


        public void OnDrag(PointerEventData _)
        {
            transform.position = Input.mousePosition;
            if (transform.GetComponent<Image>().raycastTarget) transform.GetComponent<Image>().raycastTarget = false;
        }


        public void OnEndDrag(PointerEventData _)
        {
            GameObject go = _.pointerCurrentRaycast.gameObject;
            if (go.tag == "Grid") //如果当前拖动物体下是：格子 -（没有物品）时
            {
                SetPosAndParent(transform, go.transform);
                transform.GetComponent<Image>().raycastTarget = true;
            }
            else if (go.tag == "Good") //如果是物品
            {
                SetPosAndParent(transform, go.transform.parent);                              //将当前拖动物品设置到目标位置
                go.transform.SetParent(topOfUiT);                                             //目标物品设置到 UI 顶层
                if (Math.Abs(go.transform.position.x - beginParentTransform.position.x) <= 0) //以下 执行置换动画，完成位置互换 （关于数据的交换，根据自己的工程情况，在下边实现）
                {
                    go.transform.DOMoveY(beginParentTransform.position.y, 0.3f).OnComplete(() =>
                    {
                        go.transform.SetParent(beginParentTransform);
                        transform.GetComponent<Image>().raycastTarget = true;
                    }).SetEase(Ease.InOutQuint);
                }
                else
                {
                    go.transform.DOMoveX(beginParentTransform.position.x, 0.2f).OnComplete(() =>
                    {
                        go.transform.DOMoveY(beginParentTransform.position.y, 0.3f).OnComplete(() =>
                        {
                            go.transform.SetParent(beginParentTransform);
                            transform.GetComponent<Image>().raycastTarget = true;
                        }).SetEase(Ease.InOutQuint);
                    });
                }
            }
            else //其他任何情况，物体回归原始位置
            {
                SetPosAndParent(transform, beginParentTransform);
                transform.GetComponent<Image>().raycastTarget = true;
            }
        }


        /// <summary>
        /// 设置父物体，UI位置归正
        /// </summary>
        /// <param name="t">对象Transform</param>
        /// <param name="parent">要设置到的父级</param>
        private void SetPosAndParent(Transform t, Transform parent)
        {
            t.SetParent(parent);
            t.position = parent.position;
        }
    }
}