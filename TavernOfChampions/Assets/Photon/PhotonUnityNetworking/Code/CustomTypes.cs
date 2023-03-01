// ----------------------------------------------------------------------------
// <copyright file="CustomTypes.cs" company="Exit Games GmbH">
//   PhotonNetwork Framework for Unity - Copyright (C) 2018 Exit Games GmbH
// </copyright>
// <summary>
// Sets up support for Unity-specific types. Can be a blueprint how to register your own Custom Types for sending.
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------


namespace Photon.Pun
{
    using UnityEngine;
    using Photon.Realtime;
    using ExitGames.Client.Photon;
    using System;
    using System.Linq;


    /// <summary>
    /// Internally used class, containing de/serialization method for PUN specific classes.
    /// </summary>
    internal static class CustomTypes
    {
        /// <summary>Register de/serializer methods for PUN specific types. Makes the type usable in RaiseEvent, RPC and sync updates of PhotonViews.</summary>
        internal static void Register()
        {
            PhotonPeer.RegisterType(typeof(Player), (byte) 'P', SerializePhotonPlayer, DeserializePhotonPlayer);
            PhotonPeer.RegisterType(typeof(Vector2Int), (byte) 'M', SerializeVector2Int, DeserializeVector2Int);
        }

        private static byte[] SerializeVector2Int(object customObject)
        {
            Vector2Int vec = (Vector2Int)customObject;

            byte[] xValueBytes = BitConverter.GetBytes(vec.x);
            byte[] yValueBytes = BitConverter.GetBytes(vec.y);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(xValueBytes);
                Array.Reverse(yValueBytes);
            }

            return JoinByteArrays(xValueBytes, yValueBytes);
        }
        private static object DeserializeVector2Int(byte[] serializedCustomObject)
        {
            byte[] xValueBytes = new byte[4];
            byte[] yValueBytes = new byte[4];

            Array.Copy(serializedCustomObject, 0, xValueBytes, 0, 4);
            Array.Copy(serializedCustomObject, 4, yValueBytes, 0, 4);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(xValueBytes);
                Array.Reverse(yValueBytes);
            }

            Vector2Int vector2Int = new Vector2Int();
            vector2Int.x = BitConverter.ToInt32(xValueBytes);
            vector2Int.y = BitConverter.ToInt32(yValueBytes);

            return vector2Int;
        }

        private static byte[] JoinByteArrays(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }

        #region Custom De/Serializer Methods

        public static readonly byte[] memPlayer = new byte[4];

        private static short SerializePhotonPlayer(StreamBuffer outStream, object customobject)
        {
            int ID = ((Player) customobject).ActorNumber;

            lock (memPlayer)
            {
                byte[] bytes = memPlayer;
                int off = 0;
                Protocol.Serialize(ID, bytes, ref off);
                outStream.Write(bytes, 0, 4);
                return 4;
            }
        }

        private static object DeserializePhotonPlayer(StreamBuffer inStream, short length)
        {
            if (length != 4)
            {
                return null;
            }

            int ID;
            lock (memPlayer)
            {
                inStream.Read(memPlayer, 0, length);
                int off = 0;
                Protocol.Deserialize(out ID, memPlayer, ref off);
            }

            if (PhotonNetwork.CurrentRoom != null)
            {
                Player player = PhotonNetwork.CurrentRoom.GetPlayer(ID);
                return player;
            }
            return null;
        }

        #endregion
    }
}