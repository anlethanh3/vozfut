import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';

const Confirmation = (props: { show: boolean, title: string, content: string, onSubmit: () => void, onClose: () => void }) => {
    const { show, title, content, onSubmit, onClose } = props
    return (<>
        <Modal show={show} onHide={onClose}>
            <Modal.Header closeButton>
                <Modal.Title>{title}</Modal.Title>
            </Modal.Header>
            <Modal.Body>{content}</Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onClose}>
                    No
                </Button>
                <Button variant="primary" onClick={onSubmit}>
                    Yes
                </Button>
            </Modal.Footer>
        </Modal>
    </>)
}

export default Confirmation