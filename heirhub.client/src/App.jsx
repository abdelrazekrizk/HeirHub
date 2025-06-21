import React from 'react';  
import DocumentUpload from './components/DocumentUpload';  
import Chatbot from './components/Chatbot';  
  
function App() {  
  return (  
    <div className="App" style={{ padding: "20px" }}>  
      <h1>HeirAssist MVP</h1>  
      <DocumentUpload />  
      <hr />  
      <Chatbot />  
    </div>  
  );  
}  
  
export default App;  