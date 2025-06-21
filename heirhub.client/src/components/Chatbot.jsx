import React, { useState } from 'react';  
  
function Chatbot() {  
  const [messages, setMessages] = useState([  
    { from: 'bot', text: 'Hello! Ask me anything about your legal documents.' }  
  ]);  
  const [input, setInput] = useState('');  
  const [loading, setLoading] = useState(false);  
  
  const sendMessage = async () => {  
    if (!input.trim()) return;  
  
    const userMessage = { from: 'user', text: input };  
    setMessages((msgs) => [...msgs, userMessage]);  
    setInput('');  
    setLoading(true);  
  
    try {  
      const response = await fetch('/api/chatbot/ask', {  
        method: 'POST',  
        headers: { 'Content-Type': 'application/json' },  
        body: JSON.stringify({ text: input }),  
      });  
      const data = await response.json();  
      const botMessage = { from: 'bot', text: data.answer };  
      setMessages((msgs) => [...msgs, botMessage]);  
    } catch (error) {  
      setMessages((msgs) => [...msgs, { from: 'bot', text: 'Error getting response.' }]);  
    } finally {  
      setLoading(false);  
    }  
  };  
  
  const handleKeyDown = (e) => {  
    if (e.key === 'Enter') sendMessage();  
  };  
  
  return (  
    <div style={{ border: '1px solid #ccc', padding: '10px', maxWidth: '400px' }}>  
      <h3>HeirAssist Chatbot</h3>  
      <div style={{ height: '300px', overflowY: 'auto', border: '1px solid #eee', padding: '5px' }}>  
        {messages.map((msg, idx) => (  
          <div key={idx} style={{ textAlign: msg.from === 'user' ? 'right' : 'left', marginBottom: '8px' }}>  
            <span style={{  
              display: 'inline-block',  
              padding: '8px',  
              borderRadius: '10px',  
              backgroundColor: msg.from === 'user' ? '#0078d4' : '#e5e5ea',  
              color: msg.from === 'user' ? '#fff' : '#000'  
            }}>  
              {msg.text}  
            </span>  
          </div>  
        ))}  
      </div>  
      <input  
        type="text"  
        value={input}  
        onChange={(e) => setInput(e.target.value)}  
        onKeyDown={handleKeyDown}  
        disabled={loading}  
        placeholder="Type your question..."  
        style={{ width: '100%', padding: '8px', marginTop: '10px' }}  
      />  
      <button onClick={sendMessage} disabled={loading || !input.trim()} style={{ marginTop: '5px' }}>  
        {loading ? 'Waiting...' : 'Send'}  
      </button>  
    </div>  
  );  
}  
  
export default Chatbot;  