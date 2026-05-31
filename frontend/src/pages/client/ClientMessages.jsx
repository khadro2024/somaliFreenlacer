import { useEffect, useState } from 'react';
import { messagesApi } from '../../api/client';
import Card from '../../components/ui/Card';
import Button from '../../components/ui/Button';
import PageHeader from '../../components/ui/PageHeader';
import { FormGroup, Input, Textarea } from '../../components/ui/Input';

export default function ClientMessages() {
  const [messages, setMessages] = useState([]);
  const [receiverId, setReceiverId] = useState('');
  const [text, setText] = useState('');

  useEffect(() => {
    messagesApi.inbox().then((r) => setMessages(r.data)).catch(() => {});
  }, []);

  const send = async (e) => {
    e.preventDefault();
    if (!receiverId || !text) return;
    await messagesApi.send({ receiverId: parseInt(receiverId, 10), text });
    setText('');
    const r = await messagesApi.inbox();
    setMessages(r.data);
  };

  return (
    <div>
      <PageHeader title="Messages" />
      <Card className="mb-6 max-w-lg">
        <form onSubmit={send}>
          <FormGroup label="Receiver User ID"><Input value={receiverId} onChange={(e) => setReceiverId(e.target.value)} required /></FormGroup>
          <FormGroup label="Message"><Textarea value={text} onChange={(e) => setText(e.target.value)} required /></FormGroup>
          <Button type="submit">Send</Button>
        </form>
      </Card>
      <Card>
        <div className="max-h-96 space-y-3 overflow-y-auto">
          {messages.slice(0, 20).map((m) => (
            <div key={m.messageId} className="rounded-lg border border-sfm-border/50 bg-sfm-surface/50 px-4 py-3">
              <p className="text-xs text-gray-500">{m.senderName} → {m.receiverName}</p>
              <p className="mt-1 text-gray-300">{m.text}</p>
            </div>
          ))}
        </div>
      </Card>
    </div>
  );
}
