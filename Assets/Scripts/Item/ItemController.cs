﻿using System;
using System.Collections;
using Game;
using Player;
using UnityEngine;

namespace Item
{
    [RequireComponent(typeof(Collider2D), typeof(AudioSource), typeof(SpriteRenderer))]
    public class ItemController : MonoBehaviour
    {
        internal string Id;
        internal GameManager Game;
        
        private AudioSource _audioSource;
        private SpriteRenderer _renderer;
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.tag.Equals("Player")) return;

            if (!(Game.State is RoundState state) || state.CurrentItem() == null || Game.player.State is PlayerDamagedState) return;
            // ReSharper disable once PossibleNullReferenceException
            if (state.CurrentItem() != null && !state.CurrentItem().id.Equals(Id))
                return;

            StartCoroutine(OnCollected(state));
        }

        private IEnumerator OnCollected(RoundState state)
        {
            _renderer.enabled = false;
            _audioSource.Play();
            Game.AddScore(state.PointsPerItem);
            state.NextItem();
            yield return new WaitForSeconds(_audioSource.clip.length);
            gameObject.SetActive(false);
        }
    }
}