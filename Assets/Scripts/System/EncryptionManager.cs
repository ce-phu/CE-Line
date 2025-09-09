using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using UnityEngine;



public class EncryptionManager
{
    private static string[ ] encryptionKey =
    {
        "6AKMiud6qcq6E6UmVirueFQPM6UV5BXV", //  0
    };

    private static string[ ] encryptionIV =
    {
        "H1w9d9l5oFxK1cyz", //  0
    };

    private const int keySize   = 256;
    private const int blockSize = 128;

    //  Network Data
    const bool APPLY_BASE64     = true;
    const bool APPLY_ENCRYPTION = true;

    static string sourceCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    static string[ ] n_encryptionKey =
    {
        "2M5KjkJk7bgSc2afy2DkEfr8jtCbxt87", //  0.1.0
    };
    
    static string[ ] appNet_Version =
    {
        "0.1.0",
    };



    public static string EncryptData( string _strData, int _version )
    {
        Debug.Assert( _version < encryptionKey.Length,
            "EncryptionManager.EncryptData -> _version >= encryptionKey.Length" );

        byte[ ] encbyte = null;
        byte[ ] rawData = Encoding.UTF8.GetBytes( _strData );

        using ( AesManaged aes = new AesManaged( ) )
        {
            SetAesParams( aes, encryptionKey[ _version ], encryptionIV[ _version ] );

            ICryptoTransform encryptor = aes.CreateEncryptor( aes.Key, aes.IV );

            using ( MemoryStream encryptedStream = new MemoryStream( ) )
            {
                using ( CryptoStream cryptStream =
                       new CryptoStream( encryptedStream, encryptor, CryptoStreamMode.Write ) )
                {
                    cryptStream.Write( rawData, 0, rawData.Length );
                }

                encbyte = encryptedStream.ToArray( );
            }
        }

        string result = System.Convert.ToBase64String( encbyte );

        return result;
    }



    public static string DecryptData( string _encStr, int _version )
    {
        Debug.Assert( _version < encryptionKey.Length,
            "EncryptionManager.DecryptData -> _version >= encryptionKey.Length" );

        byte[ ] decrypted = null;
        byte[ ] encData   = System.Convert.FromBase64String( _encStr );

        using ( AesManaged aes = new AesManaged( ) )
        {
            SetAesParams( aes, encryptionKey[ _version ], encryptionIV[ _version ] );

            ICryptoTransform decryptor = aes.CreateDecryptor( aes.Key, aes.IV );

            using ( MemoryStream encryptedStream = new MemoryStream( encData ) )
            {
                using ( MemoryStream decryptedStream = new MemoryStream( ) )
                {
                    using ( CryptoStream cryptoStream =
                           new CryptoStream( encryptedStream, decryptor, CryptoStreamMode.Read ) )
                    {
                        cryptoStream.CopyTo( decryptedStream );
                    }

                    decrypted = decryptedStream.ToArray( );
                }
            }
        }

        string result = Encoding.UTF8.GetString( decrypted );

        return result;
    }



    public static void SetAesParams( AesManaged aes, string key, string iv )
    {
        aes.KeySize   = keySize;
        aes.BlockSize = blockSize;
        aes.Mode      = CipherMode.CBC;
        aes.Padding   = PaddingMode.PKCS7;

        aes.Key = Encoding.UTF8.GetBytes( PaddingString( key, keySize   / 8 ) );
        aes.IV  = Encoding.UTF8.GetBytes( PaddingString( iv,  blockSize / 8 ) );
    }



    public static string PaddingString( string str, int len, char PaddingCharacter = '.' )
    {
        return str.Length < len ? str.PadRight( len, PaddingCharacter ) : str.Substring( 0, len );
    }



    public static string EncryptSendData( string _sendData )
    {
        byte[ ] encrypted = null;

        if ( APPLY_ENCRYPTION )
        {
            int versionIdx  = appNet_Version.Length - 1;

            Aes encryptor   = Aes.Create( );

            encryptor.Mode  = CipherMode.CBC;
            encryptor.Key   = Encoding.UTF8.GetBytes( n_encryptionKey[ versionIdx ] );
            encryptor.IV    = GetRandomBytes( 16 );

            using ( MemoryStream memoryStream = new MemoryStream( ) )
            {
                ICryptoTransform aesEncryptor = encryptor.CreateEncryptor( );

                using ( CryptoStream cryptoStream =
                       new CryptoStream( memoryStream, aesEncryptor, CryptoStreamMode.Write ) )
                {
                    byte[ ] plainBytes = Encoding.UTF8.GetBytes( _sendData );
                    cryptoStream.Write( plainBytes, 0, plainBytes.Length );
                    cryptoStream.FlushFinalBlock( );

                    byte[ ] tmp = memoryStream.ToArray( );
                    byte[ ] iv  = encryptor.IV;
                    encrypted = new byte[ tmp.Length + iv.Length ];
                    int count = 0;

                    for ( int i = 0; i < iv.Length; i++ )
                    {
                        encrypted[ i ] = iv[ i ];
                        count++;
                    }

                    for ( int i = 0; i < tmp.Length; i++ )
                    {
                        encrypted[ count + i ] = tmp[ i ];
                    }

                    return Convert.ToBase64String( encrypted, 0, encrypted.Length );
                }
            }
        }
        else
        {
            if ( APPLY_BASE64 )
            {
                encrypted = Encoding.UTF8.GetBytes( _sendData );
                return Convert.ToBase64String( encrypted, 0, encrypted.Length );
            }
        }

        return _sendData;
    }



    static byte[ ] GetRandomBytes( int _length )
    {
        byte[ ]       randBytes = new byte[ _length ];
        System.Random random    = new System.Random( );

        for ( int i = 0; i < randBytes.Length; i++ )
        {
            randBytes[ i ] = ( byte )sourceCharacters[ random.Next( sourceCharacters.Length ) ];
        }

        return randBytes;
    }



    public static string DecryptRecieveData( string _appNetVersion, string _encryptedData )
    {
        if ( APPLY_ENCRYPTION )
        {
            int versionIdx  = GetAppNetVersionIndex( _appNetVersion );

            byte[] tmp      = Convert.FromBase64String( _encryptedData );

            string iv       = Encoding.UTF8.GetString( tmp, 0, 16 );
            byte[] data     = new byte[ tmp.Length - 16 ];

            for ( int i = 0; i < data.Length; i++ ) {

                data[ i ]   = tmp[ i + 16 ];
            }

            Aes encryptor   = Aes.Create( );

            encryptor.Mode = CipherMode.CBC;
            encryptor.Key  = Encoding.UTF8.GetBytes( n_encryptionKey[ versionIdx ] );
            encryptor.IV   = Encoding.UTF8.GetBytes( iv );

            using ( MemoryStream memoryStream = new MemoryStream( ) )
            {
                ICryptoTransform aesDecryptor = encryptor.CreateDecryptor( );

                using ( CryptoStream cryptoStream = new CryptoStream( memoryStream, aesDecryptor, CryptoStreamMode.Write ) ) {

                    cryptoStream.Write( data, 0, data.Length );
                    cryptoStream.FlushFinalBlock( );

                    byte[ ] plainBytes  = memoryStream.ToArray( );

                    // Convert the decrypted byte array to string
                    return Encoding.UTF8.GetString( plainBytes, 0, plainBytes.Length );
                }
            }
        }
        else if ( APPLY_BASE64 )
        {
            byte[ ] tmp = Convert.FromBase64String( _encryptedData );
            return Convert.ToBase64String( tmp, 0, tmp.Length );
        }

        return _encryptedData;
    }



    static int GetAppNetVersionIndex( string _appVersion )
    {
        for ( int i = 0; i < appNet_Version.Length; i++ )
        {
            if ( appNet_Version[ i ].CompareTo( _appVersion ) == 0 )
            {
                return i;
            }
        }

        return -1;
    }



    public static string GetLatestAppNetVersion( )
    {
        return appNet_Version[ appNet_Version.Length - 1 ];
    }
}