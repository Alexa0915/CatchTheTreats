using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
public class ShopItem
{
    public string title;
    public Sprite sprite;
    public int price;
    public TMP_Text buttonText;
}

public class ShopManager : MonoBehaviour
{
    public ShopItem[] items;
    public TMP_Text totalCoinsText;
    public GameObject shopPanel;
    public GameObject menuPanel;
    public SpriteRenderer player;
    public GameObject notEnoughMessage;
    public float messageTime = 1.5f;

    Coroutine messageRoutine;

    void Start()
    {
        shopPanel.SetActive(false);
        if (notEnoughMessage != null) notEnoughMessage.SetActive(false);
        ApplySelected();
        Refresh();
    }

    bool IsOwned(int i)
    {
        return i == 0 || PlayerPrefs.GetInt("Owned_" + i, 0) == 1;
    }

    int Selected()
    {
        int s = PlayerPrefs.GetInt("Selected", 0);
        if (s < 0 || s >= items.Length) s = 0;
        return s;
    }

    public void OpenShop()
    {
        menuPanel.SetActive(false);
        shopPanel.SetActive(true);
        Refresh();
    }

    public void CloseShop()
    {
        if (notEnoughMessage != null) notEnoughMessage.SetActive(false);
        shopPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void OnItemClick(int i)
    {
        if (i < 0 || i >= items.Length) return;

        if (!IsOwned(i))
        {
            int coins = PlayerPrefs.GetInt("TotalCoins", 0);
            if (coins < items[i].price)
            {
                ShowNotEnough();
                return;
            }
            PlayerPrefs.SetInt("TotalCoins", coins - items[i].price);
            PlayerPrefs.SetInt("Owned_" + i, 1);
        }

        PlayerPrefs.SetInt("Selected", i);
        PlayerPrefs.Save();
        ApplySelected();
        Refresh();
    }

    void ShowNotEnough()
    {
        if (notEnoughMessage == null) return;
        if (messageRoutine != null) StopCoroutine(messageRoutine);
        messageRoutine = StartCoroutine(MessageRoutine());
    }

    IEnumerator MessageRoutine()
    {
        notEnoughMessage.SetActive(true);
        yield return new WaitForSecondsRealtime(messageTime);
        notEnoughMessage.SetActive(false);
    }

    void ApplySelected()
    {
        int s = Selected();
        if (player != null && items[s].sprite != null)
            player.sprite = items[s].sprite;
    }

    void Refresh()
    {
        totalCoinsText.text = "COINS: " + PlayerPrefs.GetInt("TotalCoins", 0);
        int s = Selected();

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].buttonText == null) continue;

            if (i == s) items[i].buttonText.text = "SELECTED";
            else if (IsOwned(i)) items[i].buttonText.text = "SELECT";
            else items[i].buttonText.text = items[i].price.ToString();
        }
    }

    [ContextMenu("Test: add 1000 coins")]
    void AddTestCoins()
    {
        PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins", 0) + 1000);
        PlayerPrefs.Save();
        Debug.Log("Coins: " + PlayerPrefs.GetInt("TotalCoins", 0));
    }

    [ContextMenu("Test: reset shop")]
    void ResetShop()
    {
        PlayerPrefs.DeleteKey("TotalCoins");
        PlayerPrefs.DeleteKey("Selected");
        for (int i = 0; i < 10; i++) PlayerPrefs.DeleteKey("Owned_" + i);
        PlayerPrefs.Save();
        Debug.Log("Shop reset");
    }
}