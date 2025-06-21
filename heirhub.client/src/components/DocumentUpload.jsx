import React, { useState } from 'react';  
  
function DocumentUpload() {  
  const [file, setFile] = useState(null);  
  const [summary, setSummary] = useState('');  
  const [loading, setLoading] = useState(false);  
  
  const handleFileChange = (e) => {  
    setFile(e.target.files[0]);  
    setSummary('');  
  };  
  
  const handleUpload = async () => {  
    if (!file) return alert('Please select a file first.');  
  
    setLoading(true);  
    const formData = new FormData();  
    formData.append('file', file);  
  
    try {  
      const response = await fetch('/api/document/upload', {  
        method: 'POST',  
        body: formData,  
      });  
      const data = await response.json();  
      setSummary(data.summary);  
    } catch (error) {  
      alert('Error uploading file.');  
    } finally {  
      setLoading(false);  
    }  
  };  
  
  return (  
    <div>  
      <h2>Upload Legal Document</h2>  
      <input type="file" onChange={handleFileChange} />  
      <button onClick={handleUpload} disabled={loading}>  
        {loading ? 'Uploading...' : 'Upload & Summarize'}  
      </button>  
      {summary && (  
        <div style={{ marginTop: '20px' }}>  
          <h3>Summary:</h3>  
          <p>{summary}</p>  
        </div>  
      )}  
    </div>  
  );  
}  
  
export default DocumentUpload;  