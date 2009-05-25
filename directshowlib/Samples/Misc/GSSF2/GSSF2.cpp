/****************************************************************************
This sample is released as public domain.  It is distributed in the hope that
it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty
of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*****************************************************************************/

// This sample was born as the PushSourceBitmap sample from the DXSDK.  However,
// little remains of that original code except some of the class/method names.

// See the readme.txt file for a discussion of how to use this filter

#include <streams.h>

#include "PushSource.h"
#include "PushGuids.h"

/**********************************************
 *
 *  CPushPinBitmap Class
 *
 *
 **********************************************/

CPushPinBitmap::CPushPinBitmap(HRESULT *phr, CSource *pFilter)
      : CSourceStream(NAME("Generic Sample Pin"), phr, pFilter, L"Out"),
        CSourceSeeking(NAME("Generic Sample Pin Seeking"), (IPin*) CBasePin::GetOwner(), phr, m_pFilter->pStateLock()),
        CGenericSampleConfig(NAME("Generic Sample Pin Seeking"), CBasePin::GetOwner()),
        m_Callback(NULL)
{
    m_bTryMyTypesFirst = true;
}

CPushPinBitmap::~CPushPinBitmap()
{
    if (m_Callback != NULL)
    {
        m_Callback->Release();
    }
}


// GetMediaType: This method tells the downstream pin what types we support.

// Here is how CSourceStream deals with media types:
//
// If you support exactly one type, override GetMediaType(CMediaType*). It will then be
// called when (a) our filter proposes a media type, (b) the other filter proposes a
// type and we have to check that type.
//
// If you support > 1 type, override GetMediaType(int,CMediaType*) AND CheckMediaType.
//
// In this case we support only one type.

HRESULT CPushPinBitmap::GetMediaType(CMediaType *pMediaType)
{
    HRESULT hr = S_OK;

    CheckPointer(pMediaType, E_POINTER);

    CAutoLock cAutoLock(m_pFilter->pStateLock());

    // If the bitmapinfo hasn't been loaded, just fail here.
    if (m_amt.IsValid())
    {
        hr = pMediaType->Set(m_amt);
    }
    else
    {
        hr = E_FAIL;
    }

    return hr;
}

HRESULT CPushPinBitmap::CheckMediaType(const CMediaType *pmt)
{
    HRESULT hr;

    CheckPointer(pmt, E_POINTER);
    CheckPointer(m_Callback, E_POINTER);

    CAutoLock cAutoLock(m_pFilter->pStateLock());

    hr = m_Callback->CheckMediaType(pmt);

    return hr;
}

HRESULT CPushPinBitmap::CompleteConnect(IPin *pReceivePin)
{
    HRESULT hr;

    CheckPointer(pReceivePin, E_POINTER);
    CheckPointer(m_Callback, E_POINTER);

    CAutoLock cAutoLock(m_pFilter->pStateLock());

    CMediaType cmt = m_mt;

    m_Callback->OnMediaTypeChanged(&cmt); // Ignore return
    cmt.cbFormat = 0;  // Callee must free the AMT

    hr = CBaseOutputPin::CompleteConnect(pReceivePin);

    return hr;
}

// DecideBufferSize: Calculate the size and number of buffers

HRESULT CPushPinBitmap::DecideBufferSize(IMemAllocator *pAlloc, ALLOCATOR_PROPERTIES *pRequest)
{
    HRESULT hr;

    CheckPointer(pAlloc, E_POINTER);
    CheckPointer(pRequest, E_POINTER);

    CAutoLock cAutoLock(m_pFilter->pStateLock());

    // If the bitmapinfo hasn't been loaded, just fail here.
    if (!m_amt.IsValid())
    {
        return E_FAIL;
    }

    // Ensure a minimum number of buffers
    if (pRequest->cBuffers == 0)
    {
        pRequest->cBuffers = 2;
    }

    pRequest->cbBuffer = m_mt.lSampleSize;

    ALLOCATOR_PROPERTIES Actual;
    hr = pAlloc->SetProperties(pRequest, &Actual);
    if (SUCCEEDED(hr))
    {
        // Is this allocator unsuitable?
        if (Actual.cbBuffer < pRequest->cbBuffer)
        {
            hr = E_FAIL;
        }
    }

    return hr;
}


// FillBuffer: Called by the BaseClasses once for every sample in the stream.

// It calls the callback to populate the sample

HRESULT CPushPinBitmap::FillBuffer(IMediaSample *pSample)
{
    HRESULT hr = S_OK;

    CheckPointer(pSample, E_POINTER);
    CheckPointer(m_Callback, E_POINTER);

    CAutoLock cAutoLock(m_pFilter->pStateLock());

    // Check for a media type change on the sample.  See IMediaSample::GetMediaType
    // for a description of this mechanism.
    CMediaType *pmt = NULL;
    hr = pSample->GetMediaType((AM_MEDIA_TYPE**)&pmt);
    if (hr == S_OK) // S_FALSE if the mediatype hasn't changed
    {
        hr = CheckMediaType(pmt);
        if (FAILED(hr))
        {
            DeleteMediaType(pmt);
            return E_FAIL;
        }
        else
        {
            SetMediaType(pmt);
            DeleteMediaType(pmt);

            CMediaType cmt = m_mt;
            m_Callback->OnMediaTypeChanged(&cmt);
            cmt.cbFormat = 0;  // Callee must free the AMT
        }
    }

    hr = m_Callback->SampleCallback(pSample);

    return hr;
}

// OnThreadCreate: Called by baseclasses when a stream is created

HRESULT CPushPinBitmap::OnThreadCreate(void)
{
    CheckPointer(m_Callback, E_POINTER);

    HRESULT hr = m_Callback->OnThreadCreate();

    return hr;
}

// OnThreadCreate: Called by baseclasses when a stream is shut down

HRESULT CPushPinBitmap::OnThreadDestroy(void)
{
    HRESULT hr;

    CheckPointer(m_Callback, E_POINTER);

    hr = m_Callback->OnThreadDestroy();

    return hr;
}

// Standard COM stuff done with a baseclasses flavor

STDMETHODIMP CPushPinBitmap::NonDelegatingQueryInterface(REFIID riid, void **ppv)
{
    HRESULT hr;

    // Try our local interface
    if (riid == IID_IGenericSampleConfig2)
    {
        hr = GetInterface((IGenericSampleConfig *) this, ppv);
    }
    else
    {
        // Try the classes we derive from, first CSourceStream
        hr = CSourceStream::NonDelegatingQueryInterface(riid, ppv);

        if (FAILED(hr))
        {
            // Second, CSourceSeeking
            hr = CSourceSeeking::NonDelegatingQueryInterface(riid, ppv);
        }
    }

    return hr;
}


// SetMediaType: Populate the media type from a completely constructed media type.

STDMETHODIMP CPushPinBitmap::SetPinMediaType(const AM_MEDIA_TYPE *amt)
{
    HRESULT hr = S_OK;
    CAutoLock cAutoLock(m_pFilter->pStateLock());

    // Only allow one init
    if (!IsConnected())
    {
        // Don't allow GUID_NULL for the major type (since that's what I
        // use to indicate an unpopulated MediaType).
        if (!IsEqualGUID(amt->majortype, GUID_NULL))
        {
            hr = m_amt.Set(*amt);
        }
        else
        {
            hr = MAKE_HRESULT(1, FACILITY_WIN32, ERROR_INVALID_PARAMETER);
        }
    }
    else
    {
        hr = MAKE_HRESULT(1, FACILITY_WIN32, ERROR_ALREADY_INITIALIZED);
    }

    return hr;
}

// SetBitmapCB: Set the callback.

// You must call one of the SetMediaType* methods first.  Notice
// that there is no check to ensure the CB wasn't already set.  While
// I haven't tried it, I suspect you could change this even while
// the graph is running.  Why you would want to is a more difficult
// question.

STDMETHODIMP CPushPinBitmap::SetBitmapCB(IGenericSampleCB2 *pfn)
{
    HRESULT hr = S_OK;

    CheckPointer(pfn, E_POINTER);

    CAutoLock cAutoLock(m_pFilter->pStateLock());

    // Must set the media type first
    if (m_amt.IsValid())
    {
        m_Callback = pfn;
        m_Callback->AddRef();

        // Initialize the seeking stuff
        hr = m_Callback->Startup(&m_dwSeekingCaps, &m_rtDuration.m_time);

        // If no seeking...
        if (FAILED(hr))
        {
            m_dwSeekingCaps = 0;
            m_rtDuration = 0;
            hr = S_OK;
        }

        m_rtStop = m_rtDuration;
    }
    else
    {
        hr = E_FAIL;
    }

    return hr;
}

// Our override of CSourceSeeking::GetCurrentPosition

HRESULT CPushPinBitmap::GetCurrentPosition(__out LONGLONG *pCurrent)
{
    HRESULT hr;

    // Only make the callback if Startup said IMediaSeeking is supported
    if ((m_dwSeekingCaps & AM_SEEKING_CanGetCurrentPos) != 0)
    {
        CAutoLock lock(CSourceSeeking::m_pLock);

        hr = m_Callback->GetCurrentPosition(pCurrent);
    }
    else
    {
        hr = E_NOTIMPL;
    }

    return hr;
}

// Our implementation of CSourceSeeking's abstract method

HRESULT CPushPinBitmap::ChangeStart()
{
    HRESULT hr;

    // Only make the callback if Startup said IMediaSeeking is supported
    if (m_dwSeekingCaps != 0)
    {
        {
            CAutoLock lock(CSourceSeeking::m_pLock);

            hr = m_Callback->ChangeStart(m_rtStart);
        }

        if (SUCCEEDED(hr))
        {
            UpdateFromSeek();
        }
    }
    else
    {
        hr = E_NOTIMPL;
    }

    return hr;
}

// Our implementation of CSourceSeeking's abstract method

HRESULT CPushPinBitmap::ChangeStop()
{
    HRESULT hr;

    // Only make the callback if Startup said IMediaSeeking is supported
    if (m_dwSeekingCaps != 0)
    {
        CAutoLock lock(CSourceSeeking::m_pLock);

        hr = m_Callback->ChangeStop(m_rtStop);

        if (hr == S_FALSE)
        {
            // We're already past the new stop time -- better flush the graph.
            UpdateFromSeek();
            hr = S_OK;
        }
    }
    else
    {
        hr = E_NOTIMPL;
    }

    return hr;
}
HRESULT CPushPinBitmap::ChangeRate()
{
    return E_NOTIMPL;
}

STDMETHODIMP CPushPinBitmap::UpdateFromSeek(void)
{
    if (ThreadExists())
    {
        // Next time around the loop, the worker thread will
        // pick up the position change.

        // We need to flush all the existing data - we must do that here
        // as our thread will probably be blocked in GetBuffer otherwise.
        DeliverBeginFlush();

        // Make sure we have stopped pushing data.
        Stop();

        // Complete the flush.
        DeliverEndFlush();

        // Restart the thread.
        Run();
    }

    return S_OK;
}

/**********************************************
 *
 *  CPushSourceBitmap Class
 *
 **********************************************/

CPushSourceBitmap::CPushSourceBitmap(IUnknown *pUnk, HRESULT *phr)
           : CSource(NAME("PushSourceBitmap"), pUnk, CLSID_GenericSampleSourceFilter2)
{
    // The pin magically adds itself to our pin array.
    m_pPin = new CPushPinBitmap(phr, this);

    if (phr)
    {
        if (m_pPin == NULL)
        {
            *phr = E_OUTOFMEMORY;
        }
        else
        {
            *phr = S_OK;
        }
    }
}

CPushSourceBitmap::~CPushSourceBitmap()
{
    delete m_pPin;
}


CUnknown * WINAPI CPushSourceBitmap::CreateInstance(IUnknown *pUnk, HRESULT *phr)
{
    CPushSourceBitmap *pNewFilter = new CPushSourceBitmap(pUnk, phr );

    if (phr)
    {
        if (pNewFilter == NULL)
        {
            *phr = E_OUTOFMEMORY;
        }
        else
        {
            *phr = S_OK;
        }
    }

    return pNewFilter;
}
