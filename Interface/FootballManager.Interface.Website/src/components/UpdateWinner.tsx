import Form from 'react-bootstrap/Form'
import FloatingLabel from 'react-bootstrap/FloatingLabel'
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import { useEffect, useState } from 'react';
import { TeamColorProp } from './Rivals';

export interface UpdateWinnerProp {
    teamId: number, number: number
}
const UpdateWinner = (props: { show: boolean, initial: TeamColorProp[], onSubmit: (data: UpdateWinnerProp) => void, onClose: () => void }) => {
    const { show, initial, onSubmit, onClose } = props
    const [id, setId] = useState<number>(0)
    const [number, setNumber] = useState<number>(0)
    const onValid = () => {
        onSubmit({ number: number, teamId: id })
    }

    return (<>
        <Modal show={show} onHide={onClose}>
            <Modal.Header closeButton>
                <Modal.Title>Update Team Winner</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="updateMember.Winner">
                        <FloatingLabel controlId="floatingWinnerId" label="Choose Team Winner">
                            <Form.Select value={id} onChange={(e) => setId(parseInt(e.target.value))}>
                                {
                                    initial.map((value, index) =>
                                        <option key={`WinnerId.id-${value}`} value={index}>{value.name}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="updateWinner.ChampionCount">
                        <FloatingLabel controlId="floatingDescription" label="Adding Champion Count">
                            <Form.Control type="number" placeholder="Input Champion Count" value={number} onChange={(e) => setNumber(parseInt(e.target.value))} />
                        </FloatingLabel>
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onClose}>
                    Close
                </Button>
                <Button variant="primary" onClick={onValid}>
                    Update
                </Button>
            </Modal.Footer>
        </Modal>
    </>)
}

export default UpdateWinner