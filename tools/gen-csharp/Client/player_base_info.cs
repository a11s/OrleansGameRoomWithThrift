/**
 * Autogenerated by Thrift Compiler (0.10.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace Client
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class player_base_info : TBase
  {
    private string _player_name;
    private string _map_id;
    private vector3 _last_map_pos;

    public string Player_name
    {
      get
      {
        return _player_name;
      }
      set
      {
        __isset.player_name = true;
        this._player_name = value;
      }
    }

    public string Map_id
    {
      get
      {
        return _map_id;
      }
      set
      {
        __isset.map_id = true;
        this._map_id = value;
      }
    }

    public vector3 Last_map_pos
    {
      get
      {
        return _last_map_pos;
      }
      set
      {
        __isset.last_map_pos = true;
        this._last_map_pos = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool player_name;
      public bool map_id;
      public bool last_map_pos;
    }

    public player_base_info() {
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.String) {
                Player_name = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                Map_id = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.Struct) {
                Last_map_pos = new vector3();
                Last_map_pos.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public void Write(TProtocol oprot) {
      oprot.IncrementRecursionDepth();
      try
      {
        TStruct struc = new TStruct("player_base_info");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Player_name != null && __isset.player_name) {
          field.Name = "player_name";
          field.Type = TType.String;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Player_name);
          oprot.WriteFieldEnd();
        }
        if (Map_id != null && __isset.map_id) {
          field.Name = "map_id";
          field.Type = TType.String;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Map_id);
          oprot.WriteFieldEnd();
        }
        if (Last_map_pos != null && __isset.last_map_pos) {
          field.Name = "last_map_pos";
          field.Type = TType.Struct;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          Last_map_pos.Write(oprot);
          oprot.WriteFieldEnd();
        }
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("player_base_info(");
      bool __first = true;
      if (Player_name != null && __isset.player_name) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Player_name: ");
        __sb.Append(Player_name);
      }
      if (Map_id != null && __isset.map_id) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Map_id: ");
        __sb.Append(Map_id);
      }
      if (Last_map_pos != null && __isset.last_map_pos) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Last_map_pos: ");
        __sb.Append(Last_map_pos== null ? "<null>" : Last_map_pos.ToString());
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
