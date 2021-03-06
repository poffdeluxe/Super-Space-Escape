﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace ArcadeRPG
{
    enum PlayerDir { UP, RIGHT, DOWN, LEFT }; // facing which direction
    enum enemyType { GRUNT, BERSERKER, BEETLE, TROOPER, NUM_ENEM }; // types of monsters
    enum weaponType { NONE, SWORD, LASER, GRENADE }; // types of weapons
    enum itemType { NONE, SWORD, LASER, ATT_BOOST, DEF_BOOST, KEY, NUM_ITEMS }; // types of items/boosts that can be gathered throughout the levels
    //just the basics on types
    /// <summary>
    /// Holds the data for the player
    /// </summary>
    class Player
    {
        int last_x, last_y; // "last" positions, used in updating positions
        int x; // x position
        int y; // y position
        int width, height; // dimensions
        List<Item> inventory; // all items collected
        weaponType activeWeapon; // weapon currently using
        PlayerDir dir; // direction user currently facing
        public ColToken col_tok;
        public bool moving; // is the character moving?
        public bool hurt; // is the character getting hurt?
        int attack, defense, speed, health, max_health; //Bonuses from items
        // player has an attack, speed, and health or hp stat
        // a small weapon inventory, size is to be decided
        // or a system of active/secondary weapons can be used

        /// <summary>
        /// Constructor for a Player object
        /// </summary>
        /// <param name="xLoc">Input the initial x coordinate of the player</param>
        /// <param name="yLoc">Input the initial y coordinate of the player</param>
        public Player(int xLoc, int yLoc, int pWidth, int pHeight)
        {
            last_x = x = xLoc;
            last_y = y = yLoc;
            width = pWidth;
            height = pHeight; // set player position

            max_health = health = 100; // give the user 100 "units" of health
            speed = 3; // move at 3 pixels/frame
            attack = defense = 0; // no additional boosts by default
            activeWeapon = weaponType.NONE; // user starts with no weapons by default
            dir = PlayerDir.UP; // facing up by default
            moving = false; // users not moving by default
            col_tok = null;
            inventory = new List<Item>(); // has no inventory by default
            hurt = false; // user has not been hurt yet
        }

        /// <summary>
        /// Returns the current x coordinate of the player
        /// </summary>
        /// 
        public List<Item> getInventory()
        {
            return inventory; // get all the items the user has collected
        }

        public int getX()
        {
            return x;
        }

        /// <summary>
        /// Returns the current y coordinate of the player
        /// </summary>
        
        //utility functions for the character--> most return self explanatory members
        public int getY()
        {
            return y;
        }

        public int getWidth()
        {
            return width;
        }


        public int getHeight()
        {
            return height;
        }

        public int getSpeed()
        {
            return speed;
        }

        public int getHealth()
        {
            return health;
        }

        public int getMaxHealth()
        {
            return max_health;
        }

        public weaponType getWeapon()
        {
            return activeWeapon;
        }

        public int getAttackBonus()
        {
            return attack;
        }

        public int getDefenseBonus()
        {
            return defense;
        }

        public void setAttackBonus(int _attack)
        {
            attack = _attack;
        }

        public void setDefenseBonus(int _defense)
        {
            defense = _defense;
        }
        /// <summary>
        /// Sets the current x coordinate of the player
        /// </summary>
        /// <param name="xLoc">Input the initial x coordinate of the player</param>
        public void setX(int xLoc)
        {
            last_x = x;
            x = xLoc;
            if (col_tok != null)
            {
                col_tok.update(x, y);
            }
        }


        /// <summary>
        /// Sets the current y coordinate of the player
        /// </summary>
        /// <param name="yLoc">Input the initial y coordinate of the player</param>
        public void setY(int yLoc)
        {
            last_y = y;
            y = yLoc;
            if (col_tok != null)
            {
                col_tok.update(x, y);
            }
        }

        //when player grabs a weapon this changes attack stats
        public void setWeapon(weaponType _weapon)
        {
            activeWeapon = _weapon;
        }

        public void setHealth(int _health)
        {
            health = _health; // if boost collected, change health max
        }

        public void setDirection(PlayerDir _dir)
        {
            dir = _dir; //update direction the character is facing
        }

        public PlayerDir getDirection()
        {
            return dir; //update direction the character is facing
        }

        public void addItem(Item item)
        {
            inventory.Add(item); // user has collected an item, add it to the inventory list
        }

        public void removeItem(Item item)
        {
            inventory.Remove(item); // user has used up an item in the inventory, remove it from the inventory
        }

        public bool hasKey() // has the user collected a key to open a gate in the level?
        {
            foreach (Item i in inventory)
            {
                if (i.getType() == itemType.KEY)
                    return true;
            }
            return false;
        }
    }

    class Enemy
    {
        
        //*********************************************************************************//
        //Refer to player class- most of the members are the same and have the same meaning
        //*********************************************************************************//
        int last_x, last_y;
        int x;
        int y;
        int width, height;
        int attack;
        int speed;
        int max_health, health;

        public ColToken col_tok;
        PlayerDir dir, last_dir;
        enemyType eType;
        Sprite sprite;

        public int next_think_time; // for the AI to "think"
        Node cur_target; // is the AI seeking a target?
        List<Node> path; // path for AI to travel

        public Enemy(int xLoc, int yLoc, int eWidth, int eHeight, enemyType t)
        {
            last_x = x = xLoc;
            last_y = y = yLoc;
            width = eWidth;
            height = eHeight;

            last_dir = dir = PlayerDir.UP;
            cur_target = null;
            col_tok = null;
            //creates the lowest level of enemy
            if (t == enemyType.GRUNT)
            {
                eType = t;
                attack = 2;
                speed = 2;
                max_health = health = 10;
            }
            //creates the second level of enemy
            else if (t == enemyType.BEETLE)
            {
                eType = t;
                attack = 5;
                speed = 2;
                max_health = health = 10;
            }
            //creates toughest level of enemy
            else if (t == enemyType.BERSERKER)
            {
                eType = t;
                attack = 5;
                speed = 3;
                max_health = health = 15;
            }
            else if (t == enemyType.TROOPER)
            {
                eType = t;
                attack = 5;
                speed = 2;
                max_health = health = 20;

            }


        }

        /// <summary>
        /// Returns the current x coordinate of the enemy
        /// </summary>
        public int getX()
        {
            return x;
        }

        /// <summary>
        /// Returns the current y coordinate of the enemy
        /// </summary>
        
        //more utilities like with the player, most return self-explanatory members
        public int getY()
        {
            return y;
        }

        public int getWidth()
        {
            return width;
        }


        public int getHeight()
        {
            return height;
        }


        public int getSpeed()
        {
            return speed;
        }

        public void setHealth(int new_health)
        {
            health = new_health;
        }

        public int getHealth()
        {
            return health;
        }

        public int getMaxHealth()
        {
            return max_health;
        }

        public enemyType getType()
        {
            return eType;
        }

        public Sprite getSprite()
        {
            return sprite;
        }

        public int getAttack()
        {
            return attack;
        }

        public void setSprite(Sprite _sprite)
        {
            sprite = _sprite;
        }

        public void setDirection(PlayerDir _dir)
        {
            last_dir = dir;
            dir = _dir;

        }

        public void setPath(List<Node> _path) // tell the monsters what path to travel
        {
            path = _path;
            if (path.Count() == 0)
            {
                cur_target = null;
                return;
            }
            cur_target = path.First();
            path.Remove(path.First());
        }

        public Node getTarget()
        {
            return cur_target;
        }

        public void nextTarget()
        {
            if (path.Count() == 0)
            {
                cur_target = null;
                return;
            }
            cur_target = path.First();
            path.Remove(path.First());
        }

        public PlayerDir getDirection()
        {
            return dir;
        }

        public PlayerDir getLastDirection()
        {
            return last_dir;
        }


        /// <summary>
        /// Sets the current x coordinate of the enemy
        /// </summary>
        /// <param name="xLoc">Input the initial x coordinate of the enemy</param>
        public void setX(int xLoc)
        {
            last_x = x;
            //last_y = y;
            x = xLoc;
            if (col_tok != null)
            {
                col_tok.update(x, y);
            }
        }

        /// <summary>
        /// Sets the current y coordinate of the enemy
        /// </summary>
        /// <param name="yLoc">Input the initial y coordinate of the enemy</param>
        public void setY(int yLoc)
        {
            last_y = y;
            y = yLoc;
            if (col_tok != null)
            {
                col_tok.update(x, y);
            }
        }

        public void revertX()
        {

            setX(last_x);
        }

        public void revertY()
        {
            setY(last_y);
        }

    }

    class Item
    {
        int attackBonus;
        int speedBonus;
        int defenseBonus;
        itemType type;
        int time;
        int textureID;
        //each weapon has a type and attack bonus, and an animation later on

        private string name; // name of item
        public Boolean collected; // has the user collected the item?
        private int boost; // what the item can do for the user (i.e., boost attack by 5, then boost will = 5)
        private Vector2 offset;
        private Color color; // for drawing function, keep it white for draw to draw no layer in front of item to be drawn

        //no time limit, items will last duration of level. upon level "advance", items that are ATTACK/DEFENSE/SPEED will be cleared out of item vector in main class
        private Texture2D itempic;
        private Vector2 itempos;

        
        public Item(itemType _type, int _ab, int _sb, int _db, int _time, int texID)
        {
            color = Color.White;
            offset = new Vector2(0, 0); // don't offset image once drawn on screen (for simplicity)
            switch (_type)
            {
                case itemType.ATT_BOOST: name = "Attack Boost"; break;
                case itemType.DEF_BOOST: name = "Defense Boost"; break;
                case itemType.KEY: name = "Key of Knowledge"; break;
                case itemType.LASER: name = "Laser of Death"; break;
                case itemType.SWORD: name = "Sword of Righteousness"; break;
                default: name = "Unknown"; break;
            }
            boost = 0; // placeholder
            collected = false; // user by default hasnt collected the item yet
            attackBonus = _ab;
            type = _type;
            speedBonus = _sb;
            defenseBonus = _db;
            time = _time;
            textureID = texID;
        }
        //gets the attack bonus to add on later
        public int getAttackBonus()
        {
            return attackBonus;
        }
        //distiguishes the weapon type

        public itemType getType()
        {
            return type;
        }

        public int getTexture()
        {
            return textureID;
        }

        public void loadContent(ContentManager contman)
        {
            itempic = contman.Load<Texture2D>("Pictures\\dummyitem");
        }

        public void setPos(Vector2 v2)
        {
            itempos.X = v2.X;
            itempos.Y = v2.Y;
        }

        public Vector2 getPos()
        {
            return itempos;
        }

        public Texture2D get2D()
        {
            return itempic;
        }

        public int getBoost()
        {
            return boost;
        }

        public string getName()
        {
            return name;
        }


    }//end item class


}


