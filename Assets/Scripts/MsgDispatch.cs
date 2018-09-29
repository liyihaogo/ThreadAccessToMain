using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MsgHandle(object obj);
/// <summary>
/// 消息分发及数据队列
/// </summary>
public class MsgDispatch
{
	private static MsgDispatch m_instance;
	public static MsgDispatch MInstance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = new MsgDispatch();
			}
			return m_instance;
		}
	}

	private Queue<MsgData> m_msgData = new Queue<MsgData>();
	private Dictionary<int, List<MsgHandle>> m_msgHandle = new Dictionary<int, List<MsgHandle>>();

	public void RegMsg(int mid, MsgHandle msg)
	{
		List<MsgHandle> msgLst;
		if (!m_msgHandle.TryGetValue(mid, out msgLst))
		{
			msgLst = new List<MsgHandle>();
			m_msgHandle.Add(mid, msgLst);
		}
		msgLst.Add(msg);
	}

	public void UnRegMsg(int mid, MsgHandle msg)
	{
		List<MsgHandle> msgLst;
		if (!m_msgHandle.TryGetValue(mid, out msgLst))
		{
			return;
		}
		msgLst.Remove(msg);
		if (msgLst.Count <= 0)
			m_msgHandle.Remove(mid);
	}

	public void Dispatch(int mid, object data)
	{
		List<MsgHandle> msgLst;
		if (!m_msgHandle.TryGetValue(mid, out msgLst)) return;
		int len = msgLst.Count;
		for (int i = 0; i < len; i++)
		{
			msgLst[i](data);
		}
	}

	public void Push(MsgData data)
	{
		m_msgData.Enqueue(data);
	}

	public void Update()
	{
		if (m_msgData.Count > 0)
		{
			MsgData mdata = m_msgData.Dequeue();
			MsgDispatch.MInstance.Dispatch(mdata.MMsgId, mdata.MSpeed);
		}
	}
}

public class MsgData
{
	public int MMsgId = 100;
	public float MSpeed = 0f;
}
