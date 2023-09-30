import { Button, Modal, FloatingLabel, Form } from 'react-bootstrap';
import { useState } from 'react';
import { NewsProps } from '../slices/newsSlice';

export default function AddNews(props: { show: boolean, onSubmit: (model: NewsProps) => void, onClose: () => void }) {
    const { show, onSubmit, onClose } = props
    const [news, setNews] = useState<NewsProps>({ title: '', content: '', imageUris: '', id: 0 })
    const [valid, setValid] = useState({ title: true });
    const onValid = () => {
        if (news.title === '') {
            setValid({ ...valid, title: false })
            return
        }
        onSubmit(news)
    }

    return (<>
        <Modal show={show} onHide={onClose}>
            <Modal.Header closeButton>
                <Modal.Title>Add a News</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="addNews.Title">
                        <FloatingLabel controlId="floatingTitle" label="Title">
                            <Form.Control type="text" required isInvalid={!valid.title} placeholder="Input name" value={news.title} onChange={(e) => setNews({ ...news, title: e.target.value })} />
                            <Form.Control.Feedback type="invalid">Title is required.</Form.Control.Feedback>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addNews.content">
                        <Form.Control as="textarea" rows={5} placeholder="Input content" value={news.content} onChange={(e) => setNews({ ...news, content: e.target.value })} />
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addNews.imageUris">
                        <Form.Control as="textarea"  placeholder='Input Image Ids per new line' rows={4} value={news.imageUris} onChange={(e) => setNews({ ...news, imageUris: e.target.value })} />
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onClose}>
                    Close
                </Button>
                <Button variant="primary" onClick={onValid}>
                    Add
                </Button>
            </Modal.Footer>
        </Modal>
    </>)
}