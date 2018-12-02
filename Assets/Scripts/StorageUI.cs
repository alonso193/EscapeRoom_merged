using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class StorageUI : MonoBehaviour
{

    public GameObject[] playerDBSlots;
    private bool storageState = false;
    public int currentSlot;
    public Color[] colorSlots;
    public Color[] colorIcons;
    private GameObject storage;
    private GameObject player;
    private ObjectInteractor playerInteractor;
    public Image carryImage, carryIcon;

    [SerializeField] public Images images;
    [System.Serializable]
    public class Images
    {
        public Image[] slot, icon;
    }

    [SerializeField] public Icons icons;
    [System.Serializable]
    public class Icons
    {
        public Sprite[] icon;
        public string[] tag;
    }

    private void Awake()
    {
        storage = GameObject.FindWithTag("Storage");
        storage.SetActive(storageState);
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerInteractor = player.GetComponent<ObjectInteractor>();
        }
        UpdateCurrentSlot();
    }

    void Update()
    {
        SetVisibilityState();
        if (storageState)
        {
            MoveThroughStorage();
            UpdateStorageIcons();
            UpdateCurrentSlot();
            RemoveItem();
            ChangeCarryObject();
        }
    }

    void SetVisibilityState()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            storageState = !storageState;
            storage.SetActive(storageState);
            playerInteractor.SetInteraction(!storageState);
        }
    }

    void UpdateStorageIcons()
    {
        for (int i = 0; i < images.icon.Length; i++)
        {
            if (playerDBSlots[i] != null)
            {
                for (int j = 0; j < icons.tag.Length; j++)
                {
                    if (playerDBSlots[i].tag == icons.tag[j])
                    {
                        images.icon[i].sprite = icons.icon[j];
                    }
                }
            }
            else
            {
                images.icon[i].sprite = null;
            }
        }

        if (playerInteractor.carryObject != null)
        {
            for (int i = 0; i < icons.tag.Length; i++)
            {
                if (playerInteractor.carryObject.tag == icons.tag[i])
                {
                    carryIcon.sprite = icons.icon[i];
                }
            }
        }
        else
        {
            carryIcon.sprite = null;
        }
    }

    void UpdateCurrentSlot()
    {
        for (int j = 0; j < images.slot.Length; j++)
        {
            if (j == currentSlot)
            {
                images.slot[currentSlot].color = colorSlots[0];
                images.icon[currentSlot].color = colorIcons[0];
            }
            else
            {
                images.slot[j].color = colorSlots[1];
                images.icon[j].color = colorIcons[1];

            }
        }
    }

    void MoveThroughStorage()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            if (currentSlot < playerDBSlots.Length - 1)
                currentSlot++;
            else
                currentSlot = 0;
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            if (currentSlot > 0)
                currentSlot--;
            else
                currentSlot = playerDBSlots.Length - 1;
        }
    }

    void RemoveItem()
    {
        if (Input.GetButtonDown("Button_Circle"))
        {
            if (playerDBSlots[currentSlot] != null)
            {
                playerDBSlots[currentSlot].SetActive(true);
                playerDBSlots[currentSlot].GetComponent<IInteractive>().ExecuteCarryAction("Drop", player);
                playerDBSlots[currentSlot] = null;
            }
        }
    }

    void ChangeCarryObject()
    {
        if (Input.GetButtonDown("Button_X"))
        {
            GameObject temp = playerInteractor.carryObject;

            playerInteractor.carryObject = playerDBSlots[currentSlot];
            if (playerInteractor.carryObject != null)
            {
                playerInteractor.carryObject.SetActive(true);
            }

            playerDBSlots[currentSlot] = temp;
            if (playerDBSlots[currentSlot] != null)
            {
                playerDBSlots[currentSlot].SetActive(false);
            }
        }
    }

    public bool GetEmptySlots()
    {
        for (int i = 0; i < playerDBSlots.Length; i++)
        {
            if (playerDBSlots[i] == null)
                return true;
        }
        return false;
    }

    public void UpdateStorage(GameObject obj)
    {
        for (int i = currentSlot; i <= playerDBSlots.Length; i++)
        {
            if (GetEmptySlots())
            {
                if (i == playerDBSlots.Length)
                {
                    i = -1;
                }
                else if (playerDBSlots[i] == null)
                {
                    playerDBSlots[i] = obj;
                    return;
                }
            }
        }
    }

    public bool GetStorageState()
    {
        return storageState;
    }

}