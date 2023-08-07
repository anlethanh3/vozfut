import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';

const Confirmation = ({ show, title, content, onSubmit, onClose }) => {

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