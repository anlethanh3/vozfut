import { Button, Modal, FloatingLabel, Form } from 'react-bootstrap';
import { useState } from 'react';
import { MatchProps } from '../slices/matchSlice';

export default function AddMatch(props: { show: boolean, onSubmit: (model: MatchProps) => void, onClose: () => void }) {
    const { show, onSubmit, onClose } = props
    const [match, setMatch] = useState<MatchProps>({ name: '', description: '', teamCount: 3, teamSize: 5, id: 0 })
    const [valid, setValid] = useState({ name: true });
    const onValid = () => {
        if (match.name === '') {
            setValid({ ...valid, name: false })
            return
        }
        onSubmit(match)
    }

    return (<>
        <Modal show={show} onHide={onClose}>
            <Modal.Header closeButton>
                <Modal.Title>Add a match</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="addMatch.Name">
                        <FloatingLabel controlId="floatingName" label="Name">
                            <Form.Control type="text" required isInvalid={!valid.name} placeholder="Input name" value={match.name} onChange={(e) => setMatch({ ...match, name: e.target.value })} />
                            <Form.Control.Feedback type="invalid">Name is required.</Form.Control.Feedback>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addMatch.Description">
                        <FloatingLabel controlId="floatingDescription" label="Description">
                            <Form.Control type="text" placeholder="Input description" value={match.description} onChange={(e) => setMatch({ ...match, description: e.target.value })} />
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addMatch.TeamCount">
                        <FloatingLabel controlId="floatingTeamCount" label="TeamCount">
                            <Form.Select value={match.teamCount} onChange={(e) => setMatch({ ...match, teamCount: parseInt(e.target.value) })}>
                                {
                                    [2, 3, 4,5,6,7,8].map(value =>
                                        <option key={`teamcount.id-${value}`} value={value}>{value}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addMatch.TeamSize">
                        <FloatingLabel controlId="floatingTeamSize" label="TeamSize">
                            <Form.Select value={match.teamSize} onChange={(e) => setMatch({ ...match, teamSize: parseInt(e.target.value) })}>
                                {
                                    [5,6, 7,8,9,10, 11].map(value =>
                                        <option key={`teamcount.id-${value}`} value={value}>{value}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
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